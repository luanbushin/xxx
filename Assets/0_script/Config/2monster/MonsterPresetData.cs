using Game.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    public class MonsterPresetDataValueData : CsvValueParser<MonsterPresetDataValueData>
    {
        public int ID { get; private set; }
        public string NAME { get; private set; }
        public float move { get; private set; }
        public int activation { get; private set; }
        public int activerange { get; private set; }
        public int losttargettime { get; private set; }
        public int viewrange { get; private set; }
        public int viewagent { get; private set; }
        public int attackrange { get; private set; }
        public int attacktype { get; private set; }
        public int dps { get; private set; }
        public int attackbuffId { get; private set; }
        public int hp { get; private set; }

        public override MonsterPresetDataValueData parse(CsvValue v)
        {
            ID = v.getInt("ID");
            NAME = v.getStr("NAME");
            move = v.getFloat("move");
            activation = v.getInt("activation");
            activerange = v.getInt("activerange");
            losttargettime = v.getInt("losttargettime");
            viewrange = v.getInt("viewrange");
            viewagent = v.getInt("viewagent");
            attackrange = v.getInt("attackrange");
            attacktype = v.getInt("attacktype");
            dps = v.getInt("dps");
            attackbuffId = v.getInt("attackbuffId");
            hp = v.getInt("hp");

            return this;
        }

        void Update()
        {

        }
    }

    public class MonsterPresetData : CsvData1<MonsterPresetData, MonsterPresetDataValueData>
    {
        protected override string fileName { get { return "MonsterPreset.csv"; } }
    }
}
