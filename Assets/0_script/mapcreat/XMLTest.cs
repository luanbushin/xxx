using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;

public class XMLTest
{




    public PassInfo loadPassInfo() {
        PassInfo info = new PassInfo();
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
        xml.Load(XmlReader.Create((Application.streamingAssetsPath + "/passInfo.xml"), set));

        //得到objects节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("Map").ChildNodes;

        //遍历所有子节点
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.Name == "born")
            {
                info.setborn(xl1.InnerXml);
            } else if (xl1.Name == "completeType") {
                info.passtye =xl1.InnerXml; 
            }
            else if (xl1.Name == "targetVetor3")
            {
                info.setTraget(xl1.InnerXml);
            }
            else if (xl1.Name == "trap")
            {
                info.settrap(xl1.InnerXml);
            }
        }

        return info;
    }
    public IEnumerator LoadEnemyXml(Dictionary<Vector3, int> mapIndexObj,Dictionary<Vector3, Dictionary<Vector3, PatrolData>> patrplData)
    {
        //创建xml文档
        /**XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
        xml.Load(XmlReader.Create((Application.dataPath + "/StreamingAssets/enemy.xml"), set));*/
        
        string localPath;
        if (Application.platform == RuntimePlatform.Android)
        {
            localPath = UnityEngine.Application.streamingAssetsPath + "/enemy.xml"; 
        }
        else
        {
            localPath = "file://" + UnityEngine.Application.streamingAssetsPath + "/enemy.xml";
        }
        //txt.text = localPath;
        WWW www = new WWW(localPath);

        while (!www.isDone)
        {
            yield return www;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(www.text);
        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("Map").ChildNodes;
        

        //遍历所有子节点
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.Name == "enemy")
            {
                string[] itemlist = xl1.InnerXml.Split('=');
                for (int i = 0; i < itemlist.Length - 1; i++)
                {
                    string[] list = itemlist[i].Split(',');
                    mapIndexObj[new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]))] = int.Parse(list[3]);

                    if(list[4] != "null") {
                        patrplData[new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]))] = new Dictionary<Vector3, PatrolData>();
                        Dictionary<Vector3, PatrolData> patrollist = patrplData[new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]))];
                        string[] alist = list[4].Split('a');
                        for (int j = 0; j < alist.Length - 1; j++) {
                            string[] blist = alist[j].Split('b');
                            Vector3 v3 = new Vector3(int.Parse(blist[0]), int.Parse(blist[1]), int.Parse(blist[2]));
                            if (blist[3] == "null") {
                                patrollist[v3] = null;
                            }
                            else{
                                patrollist[v3] = new PatrolData();
                                string[] clist = blist[3].Split('c');
                                patrollist[v3].stayTime = int.Parse(clist[0]);
                                patrollist[v3].agentmin = int.Parse(clist[1]);
                                patrollist[v3].agentmax = int.Parse(clist[2]);
                                patrollist[v3].loop = int.Parse(clist[3]);
                            }
                        }
                    }
                }
            }
        }
        
        Game.Noticfacation.Notice.EnemyComplete.broadcast();
    }

    public Dictionary<Vector3, int> LoadEnemyXml111( Dictionary<Vector3, Dictionary<Vector3, PatrolData>> patrplData)
    {
        Dictionary<Vector3, int> mapIndexObj = new Dictionary<Vector3, int>();
        //创建xml文档
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
        xml.Load(XmlReader.Create((Application.dataPath + "/StreamingAssets/enemy.xml"), set));

        XmlNodeList xmlNodeList = xml.SelectSingleNode("Map").ChildNodes;


        //遍历所有子节点
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.Name == "enemy")
            {
                string[] itemlist = xl1.InnerXml.Split('=');
                for (int i = 0; i < itemlist.Length - 1; i++)
                {
                    string[] list = itemlist[i].Split(',');
                    mapIndexObj[new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]))] = int.Parse(list[3]);

                    if (list[4] != "null")
                    {
                        patrplData[new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]))] = new Dictionary<Vector3, PatrolData>();
                        Dictionary<Vector3, PatrolData> patrollist = patrplData[new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]))];
                        string[] alist = list[4].Split('a');
                        for (int j = 0; j < alist.Length - 1; j++)
                        {
                            string[] blist = alist[j].Split('b');
                            Vector3 v3 = new Vector3(int.Parse(blist[0]), int.Parse(blist[1]), int.Parse(blist[2]));
                            if (blist[3] == "null")
                            {
                                patrollist[v3] = null;
                            }
                            else
                            {
                                patrollist[v3] = new PatrolData();
                                string[] clist = blist[3].Split('c');
                                patrollist[v3].stayTime = int.Parse(clist[0]);
                                patrollist[v3].agentmin = int.Parse(clist[1]);
                                patrollist[v3].agentmax = int.Parse(clist[2]);
                                patrollist[v3].loop = int.Parse(clist[3]);
                            }
                        }
                    }
                }
            }
        }
        return mapIndexObj;
    }


    public Dictionary<Vector3, int> LoadBoxXml()
    {
        Dictionary<Vector3, int> mapIndexObj = new Dictionary<Vector3, int>();
        //创建xml文档
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
        xml.Load(XmlReader.Create((Application.dataPath + "/StreamingAssets/box.xml"), set));

        //得到objects节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("Map").ChildNodes;

        //遍历所有子节点
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.Name == "item")
            {
                string[] itemlist = xl1.InnerXml.Split('=');
                for (int i = 0; i < itemlist.Length - 1; i++)
                {
                    string[] list = itemlist[i].Split(',');
                    mapIndexObj[new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]))] = 11;
                }
            }
        }
        return mapIndexObj;
    }

    public IEnumerator LoadXmland(Dictionary<Vector3, int> mapIndexObj,Text txt = null)
    {
        //Dictionary<Vector3, int> mapIndexObj = new Dictionary<Vector3, int>();
        //创建xml文档
        // XmlDocument xml = new XmlDocument();
        //XmlReaderSettings set = new XmlReaderSettings();
        //set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
        //xml.Load(XmlReader.Create((Application.dataPath + "/StreamingAssets/pass.xml"), set));
        txt.text = "======================================";

        string localPath;
        if (Application.platform == RuntimePlatform.Android)
        {
            localPath = UnityEngine.Application.streamingAssetsPath + "/pass.xml"; //在Android中实例化WWW不能在路径前面加"file://"
        }
        else
        {
            localPath = "file://" + UnityEngine.Application.streamingAssetsPath + "/pass.xml";//在Windows中实例化WWW必须要在路径前面加"file://"
        }
        //txt.text = localPath;
        WWW www = new WWW(localPath);
        while (!www.isDone)
        {
            yield return www;
        }

        //txt.text = www.ToString();
        //txt.text = www.text + "===================";

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(www.text);
        //xmlDoc.LoadXml(www.text);
        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("Map").ChildNodes;
        //XmlNodeList xmlNodeList = xml.SelectSingleNode("Map").ChildNodes;


        //遍历所有子节点
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.Name == "item")
            {
                string[] itemlist = xl1.InnerXml.Split('=');
                for (int i = 0; i < itemlist.Length - 1; i++)
                {
                    string[] list = itemlist[i].Split(',');
                    mapIndexObj[new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]))] = int.Parse(list[3]);
                }
            }
        }

        Game.Noticfacation.Notice.MAP_COMPLETE_COMPLETE.broadcast();
        //得到objects节点下的所有子节点

    }

    public Dictionary<Vector3, int> LoadXml(Text txt = null)
    {
        Dictionary<Vector3, int> mapIndexObj = new Dictionary<Vector3, int>(); 
          //创建xml文档
         XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
        xml.Load(XmlReader.Create((Application.dataPath + "/StreamingAssets/pass.xml"), set));


        //得到objects节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("Map").ChildNodes;
        if (txt)
            txt.text = xmlNodeList.Count + "";

        //遍历所有子节点
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.Name == "item")
            {
                string[] itemlist = xl1.InnerXml.Split('=');
                for (int i = 0; i < itemlist.Length-1; i++)
                {
                    string[] list = itemlist[i].Split(',');
                    mapIndexObj[new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]))] = int.Parse(list[3]);
                }
            }
        }
        return mapIndexObj;
    }

    public void creatMap(int width,int height, Dictionary<Vector3, GameObject> mapObject, Dictionary<Vector3, int>mapindexObject) {
        XmlDocument xml = CreateXML();

        //获取根节点
        XmlNode root = xml.SelectSingleNode("Map");
        //添加元素
        //XmlElement element = xml.CreateElement("width");
        //element.InnerText = width+"";
        //root.AppendChild(element);

        //XmlElement element1 = xml.CreateElement("height");
        //element1.InnerText = height + "";
        //root.AppendChild(element1);
        string itemStr = "";

        foreach (Vector3 list in mapObject.Keys) {
            itemStr += list.x + "," + list.y +"," + list.z + ","+ mapindexObject[list] + "=";
        }

        XmlElement element2 = xml.CreateElement("item");
        element2.InnerText = itemStr + "";
        root.AppendChild(element2);

        //UpdateNodeToXML();
        SaveXML(xml);
    }

    public void creatEnemy(Dictionary<Vector3, GameObject> enemyObject, Dictionary<Vector3, GameObject> boxObject, Dictionary<Vector3, int> enemyIdObject, Dictionary<Vector3, GameObject> bornObject, string passtype, string targetV3, Dictionary<Vector3, Dictionary<Vector3, PatrolData>> patrplData, List<string> trapList)
    {
        XmlDocument xml = CreateXML();

        //获取根节点
        XmlNode root = xml.SelectSingleNode("Map");
        //添加元素
        string itemStr = "";

        foreach (Vector3 list in enemyObject.Keys)
        {
            string str = "null";
            if (patrplData.ContainsKey(list)) {
                str = "";

                foreach (Vector3 plist in patrplData[list].Keys) {
                    string s = "null";
                    if (patrplData[list][plist] != null) {
                        s = patrplData[list][plist].stayTime + "c" + patrplData[list][plist].agentmin + "c" + patrplData[list][plist].agentmax + "c" + patrplData[list][plist].loop;
                    }
                    str += plist.x + "b" + plist.y + "b" + plist.z + "b" + s +"a";
                }
            }
              
            itemStr += list.x + "," + list.y + "," + list.z + "," + enemyIdObject[list] +","+ str + "=";
        }

        XmlElement element2 = xml.CreateElement("enemy");
        element2.InnerText = itemStr + "";
        root.AppendChild(element2);

        SaveEnemyXML(xml);

        xml = CreateXML();

        //获取根节点
        root = xml.SelectSingleNode("Map");

        //添加元素
        XmlElement element = xml.CreateElement("completeType");
        element.InnerText = passtype + "";
        root.AppendChild(element);

        XmlElement element1 = xml.CreateElement("targetVetor3");
        element1.InnerText = targetV3 + "";
        root.AppendChild(element1);

        //添加元素
        itemStr = "";

        foreach (Vector3 list in bornObject.Keys)
        {
            itemStr += list.x + "," + list.y + "," + list.z +  "=";
        }

        element2 = xml.CreateElement("born");
        element2.InnerText = itemStr + "";
        root.AppendChild(element2);

        itemStr = "";

        for (int i = 0; i < trapList.Count; i++)
        {
            itemStr += trapList[i] + "t";
        }
        element2 = xml.CreateElement("trap");
        element2.InnerText = itemStr + "";
        root.AppendChild(element2);
        SavePassInfoXML(xml);

        //UpdateNodeToXML();

        xml = CreateXML();

        //获取根节点
        root = xml.SelectSingleNode("Map");
        //添加元素
        itemStr = "";

        foreach (Vector3 list in boxObject.Keys)
        {
            itemStr += list.x + "," + list.y + "," + list.z + "=";
        }

        element2 = xml.CreateElement("item");
        element2.InnerText = itemStr + "";
        root.AppendChild(element2);

        //UpdateNodeToXML();
        SaveBoxXML(xml);
    }


    public void creatGroup(List<List<CreatMapItem>> grouplist)
    {
        XmlDocument xml = CreateXML();

        //获取根节点
        XmlNode root = xml.SelectSingleNode("Map");
        //添加元素
        //XmlElement element = xml.CreateElement("width");
        //element.InnerText = width+"";
        //root.AppendChild(element);

        //XmlElement element1 = xml.CreateElement("height");
        //element1.InnerText = height + "";
        //root.AppendChild(element1);
        string itemStr = "";

        foreach (List<CreatMapItem> list in grouplist)
        {
            foreach (CreatMapItem item in list) {
                itemStr += item.v3.x + "," + item.v3.y + "," + item.v3.z + "," + item.typeindex + "=";
            }
            itemStr += "!";
        }

        XmlElement element2 = xml.CreateElement("item");
        element2.InnerText = itemStr + "";
        root.AppendChild(element2);

        //UpdateNodeToXML();
        SaveGroup(xml);
    }


    public List<List<CreatMapItem>> LoadGroup()
    {
        List<List<CreatMapItem>> grouplist = new List<List<CreatMapItem>>();
        //创建xml文档
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
        xml.Load(XmlReader.Create((Application.dataPath + "/StreamingAssets/group.xml"), set));

        //得到objects节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("Map").ChildNodes;

        //遍历所有子节点
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.Name == "item")
            {
                string[] itemlist = xl1.InnerXml.Split('!');
                for (int i = 0; i < itemlist.Length - 1; i++)
                {
                    List<CreatMapItem> gList = new List<CreatMapItem>();
                    string[] item1list = itemlist[i].Split('=');
                    for (int j = 0; j < item1list.Length - 1; j++)
                    {
                        CreatMapItem item = new CreatMapItem();
                        string[] list = item1list[j].Split(',');
                        item.v3 = new Vector3(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]));
                        item.typeindex = int.Parse(list[3]);
                        gList.Add(item);
                    }

                    grouplist.Add(gList);

                }
            }
        }
        return grouplist;
    }

    XmlDocument CreateXML()
    {
        //新建xml对象
        XmlDocument xml = new XmlDocument();
        //加入声明
        xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
        //加入根元素
        xml.AppendChild(xml.CreateElement("Map"));
        return xml;
    }

    
    void AddNodeToXML(XmlDocument xml, string titleValue, string infoValue)
    {
        //获取根节点
        XmlNode root = xml.SelectSingleNode("Root");
        //添加元素
        XmlElement element = xml.CreateElement("Node");
        element.SetAttribute("Type", "string");

        //在Node节点下添加子节点
        XmlElement titleElelment = xml.CreateElement("Title");
        //titleElelment.SetAttribute("Title", TitleValue);
        titleElelment.InnerText = titleValue;

        XmlElement infoElement = xml.CreateElement("Info");
        //infoElement.SetAttribute("Info", infoValue);
        infoElement.InnerText = infoValue;

        element.AppendChild(titleElelment);
        element.AppendChild(infoElement);
        root.AppendChild(element);
    }

    void UpdateNodeToXML()
    {
        string filepath = Application.dataPath + @"/INFO.XML";
        if (File.Exists(filepath))
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filepath);  //根据指定路径加载xml
            XmlNodeList nodeList = xmldoc.SelectSingleNode("Root").ChildNodes; //Node节点
            //遍历所有子节点
            foreach (XmlElement xe in nodeList)
            {
                //拿到节点中属性Type=“string”的节点
                if (xe.GetAttribute("Type") == "string")
                {
                    //更新节点属性
                    xe.SetAttribute("type", "text");
                    //继续遍历
                    foreach (XmlElement xelement in xe.ChildNodes)
                    {
                        if (xelement.Name == "TitleNode")
                        {
                            //修改节点名称对应的数值，而上面的拿到节点连带的属性
                            //xelement.SetAttribute("Title", "企业简介");
                            xelement.InnerText = "企业简介";
                        }
                    }
                    break;
                }
            }
            xmldoc.Save(filepath);
           // print("Update XML OK!");
        }
    }

    void SaveXML(XmlDocument xml)
    {
        //存储xml文件
        xml.Save(Application.dataPath + "/StreamingAssets/pass.XML");
        xml.Save(Application.persistentDataPath + "/pass.xml");
    }
    

    void SavePassInfoXML(XmlDocument xml)
    {
        //存储xml文件
        xml.Save(Application.dataPath + "/StreamingAssets/passInfo.XML");
        xml.Save(Application.persistentDataPath + "/passInfo.xml");
    }

    void SaveEnemyXML(XmlDocument xml)
    {
        //存储xml文件
        xml.Save(Application.dataPath + "/StreamingAssets/enemy.XML");
        xml.Save(Application.persistentDataPath + "/enemy.xml");
    }
    void SaveBoxXML(XmlDocument xml)
    {
        //存储xml文件
        xml.Save(Application.dataPath + "/StreamingAssets/box.XML");
        xml.Save(Application.persistentDataPath + "/box.xml");
    }

    void SaveGroup(XmlDocument xml) {
        //存储xml文件
        xml.Save(Application.dataPath + "/StreamingAssets/group.XML");
        xml.Save(Application.persistentDataPath + "/group.xml");
    }
}