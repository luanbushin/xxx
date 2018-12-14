using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Game.UI.Extension;

namespace Game.Global
{
    public class Assets : MonoSingleton<Assets>
    {
        public delegate void BundleHandler(AssetBundle assetBundle);
        public delegate void OnPrefabGameObject(GameObject object_, string name);
        public delegate void OnProgress(float progress, bool complete); // 0 ~ 1

        public bool useResource = true;

        private void visitBundle(WWW www, BundleHandler cb)
        {
            if (string.IsNullOrEmpty(www.error))
            {
                cb(www.assetBundle);
            }
            else
            {
                Debug.LogError(www.error);
            }
        }

        private AssetBundleManifest _dependencies_manifest;
        // 加载总资源依赖信息
        private IEnumerator loadDependenciesManifest()
        {
            string url = Config.PathInfo.BUNDLE_URL + "bundles";
            WWW www = new WWW(url);
            yield return www;
            visitBundle(www, (AssetBundle bundle) =>
            {
                _dependencies_manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                bundle.Unload(false);
            });
        }

        // 加载单一资源依赖bundle
        private IEnumerator loadDependencies(string bundle_name, BundleHandler cb)
        {
            string[] dependencies = _dependencies_manifest.GetAllDependencies(bundle_name);
            int len = dependencies.Length;

            for (int i = 0; i < len; ++i)
            {
                WWW www = new WWW(Config.PathInfo.BUNDLE_URL + dependencies[i]);
                yield return www;
                visitBundle(www, (AssetBundle bundle) =>
                {
                    bundle.LoadAllAssets();
                    cb(bundle);
                });
            }
        }

        public void loadSprite(string path, Image img)
        {
            img.sprite = Resources.Load<Sprite>(path);
        }

        public void loadSprite(string path, BaseImage img)
        {
            img.sprite = Resources.Load<Sprite>(path);
        }

        private void onProgressEmpty(float progress, bool complete) { }

        public void loadPrefab(string bundle_name, OnPrefabGameObject cb)
        {
            if (useResource)
                StartCoroutine(loadWithResources(new string[] { bundle_name }, cb, onProgressEmpty));
            else
                StartCoroutine(loadWithBundles(new string[] { bundle_name }, cb, onProgressEmpty));
        }

        public void loadPrefabs(string[] bundle_names, OnPrefabGameObject cb, OnProgress op)
        {
            if (useResource)
                StartCoroutine(loadWithResources(bundle_names, cb, op));
            else
                StartCoroutine(loadWithBundles(bundle_names, cb, op));
        }

        private IEnumerator loadWithResources(
            string[] bundle_names, OnPrefabGameObject cb, OnProgress op)
        {
            op(0, false);
            yield return new WaitForEndOfFrame();
            float total = bundle_names.Length;
            float process = 0;

            int len = bundle_names.Length;
            for (int i = 0; i < len; ++i)
            {
                GameObject obj = Resources.Load<GameObject>(bundle_names[i]);
                if (obj == null)
                {
                    cb(null, bundle_names[i]);
                }
                else
                {
                    cb(Instantiate(obj), bundle_names[i]);
                }
                op((++process) / total, false);
                yield return new WaitForEndOfFrame();
            }
            op(1, true);
        }
        
        // 加载prefab队列
        private IEnumerator loadWithBundles(
            string[] bundle_names, OnPrefabGameObject cb, OnProgress op)
        {
            op(0, false);
            float total = bundle_names.Length;
            float process = 0;

            if (_dependencies_manifest == null)
                yield return loadDependenciesManifest();

            int len = bundle_names.Length;
            for (int i = 0; i < len; ++i)
            {
                string name = bundle_names[i] + ".prefab.unity3d";
                WWW www = new WWW(Config.PathInfo.BUNDLE_URL + name.ToLower());
                yield return www;

                List<AssetBundle> _bundles_to_unload = new List<AssetBundle>();
                AssetBundle _load_target = null;
                visitBundle(www, (AssetBundle bundle) =>
                {
                    _load_target = bundle;
                    _bundles_to_unload.Add(bundle);
                });

                yield return loadDependencies(name, (AssetBundle bundle) =>
                {
                    _bundles_to_unload.Add(bundle);
                });

                GameObject obj =
                    _load_target.LoadAsset<GameObject>("Assets/" + bundle_names[i]);
                if (obj == null)
                {
                    cb(null, bundle_names[i]);
                }
                else
                {
                    cb(Instantiate(obj), bundle_names[i]);
                }
                op((++process) / total, false);

                foreach (AssetBundle bundle in _bundles_to_unload)
                {
                    bundle.Unload(false);
                }
            }
            op(1, true);
        }
    }
}