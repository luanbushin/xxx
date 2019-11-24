using NE = Game.Noticfacation.NEvt_0;
using NEF = Game.Noticfacation.NEvt_1<float>;
using NEI = Game.Noticfacation.NEvt_1<int>;
using NELIST = Game.Noticfacation.NEvt_1<int[,]>;
using NEFF = Game.Noticfacation.NEvt_2<float, float>;
using NEII = Game.Noticfacation.NEvt_2<int, int>;
using NES = Game.Noticfacation.NEvt_1<string>;
using NEL = Game.Noticfacation.NEvt_1<long>;
using NEV3 = Game.Noticfacation.NEvt_1<UnityEngine.Vector3>;
using NEBS = Game.Noticfacation.NEvt_2<bool, string>;
using NEGS = Game.Noticfacation.NEvt_2<string, UnityEngine.GameObject>;
using NEVS = Game.Noticfacation.NEvt_2<string, System.Collections.Generic.List<UnityEngine.Vector3>>;



namespace Game.Noticfacation
{
    public class Notice
    {
        public static readonly NE GAME_INIT_COMPLETE = new NE();
        public static readonly NE GAME_PLAYER_ATIVE = new NE();
        public static readonly NELIST MAZE_CREAT_COMPLETE = new NELIST();

        public static readonly NEF UPDATE_PHYSICS = new NEF();
        public static readonly NEF UPDATE_FRAME = new NEF();
        public static readonly NEF UPDATE_LATER = new NEF();
        public static readonly NE ON_PER_SECOND = new NE();

        public static readonly NEFF CTRL_MOVE = new NEFF();
        public static readonly NE CTRL_STOP_MOVE = new NE();
        public static readonly NE CTRL_ATTACK_START = new NE();
        public static readonly NE CTRL_ATTACK_END = new NE();

        public static readonly NEF GAME_UPDATE = new NEF();

        public static readonly NEBS ON_LOGIN = new NEBS();
        public static readonly NE START_LOADING = new NE();
        public static readonly NE INIT_HERO_LIST = new NE();
        public static readonly NE ON_SINGLE_MATCH = new NE();
        public static readonly NES ON_SINGLE_MATCH_FAILED = new NES();
        public static readonly NEL ON_MATCH_FOUND = new NEL();
        public static readonly NEI LOADING_COUNT_DOWN = new NEI();

        public static readonly NE HOST_BORN = new NE();

        public static readonly NEII ON_HUNTER_SCORE = new NEII();

        public static readonly NEI SHOW_RECEIVE_SIZE = new NEI();
        public static readonly NEI SHOW_SEND_SIZE = new NEI();

        public static readonly NEI INIT_SKILL_ICON = new NEI();


        public static readonly NE STRAT_BUILDING = new NE();

        public static readonly NES SHOW_WARING = new NES();

        public static readonly NEV3 CLICK_MONSTER = new NEV3();
        public static readonly NEI CREAT_MONSTER = new NEI();
        public static readonly NES CREAT_Trap = new NES();

        public static readonly NES Click_Use_Skill = new NES();

        public static readonly NE EnemyComplete = new NE();


        /**战斗*/
        public static readonly NEGS EnemyCreat = new NEGS();
        public static readonly NES WeaponCollision = new NES();
        public static readonly NES TrapCollision = new NES();
        public static readonly NES TrapCtrl = new NES();
        public static readonly NEVS TrapCtrl_Vector = new NEVS();
        public static readonly NEGS MonsterBeATK = new NEGS();
        public static readonly NEGS MonsterATK = new NEGS();
    }
}
