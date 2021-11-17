using Game.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Config
{
    public class PropDataValueData : CsvValueParser<PropDataValueData>
    {
        public int ID { get; private set; }
        public string NAME { get; private set; }
        public string image { get; private set; }
        public string souce { get; private set; }
        public string des { get; private set; }

        public override PropDataValueData parse(CsvValue v)
        {
            ID = v.getInt("ID");
            NAME = v.getStr("NAME");
            image = v.getStr("image");
            souce = v.getStr("souce");
            des = v.getStr("des");

            return this;
        }

        void Update()
        {

        }
    }
    public class PropData : CsvData1<PropData, PropDataValueData>
    {
        protected override string fileName { get { return "Item.csv"; } }
    }

}