
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapConfig
{
    public string[] boxNames;
    public GameObject[] mapItemList;

    public string[] flowerNames;
    public GameObject[] flowersList;
    

    public MapConfig() {
        getBoxNames();
    }
    private void getBoxNames()
    {
        string boxname = "";
        string flower = "";
        string fullPath = Application.dataPath+"/Resources/map/Prefabs" + "/";
        int i;


        boxname = "Box_1;Box_2;Box_3; Box_4; Box_A5; Box_A6; Box_A7; Box_A8; Box_B1; Box_B2; Box_B3; Box_B4; Box_B5; Box_B6; Box_B7; Box_B8; Box_C1; Box_C2; Box_C3; Box_C4; Box_C5; Box_C6; Box_C7; Box_C8; Box_D1; Box_D2; Box_D3; Box_D4; Box_D5; Box_D5_Stair; Box_D6; Box_D6_Stair; Box_D7; Box_D8; Box_E1; Box_E2; Box_E3; Box_E4; Box_E5; Box_E6; Box_E7; Box_E8; Box_F1; Box_F10; Box_F2; Box_F3; Box_F4; Box_F5; Box_F6; Box_F7; Box_F8; Box_F9; Box_M1; Box_M1B; Box_M1C; Box_M1D; Box_M1_Stair; Box_M1_StairB; Box_M2; Box_M2B; Box_M2C; Box_M2D; Box_M2_Stair; Box_M2_StairB; Box_M3; Box_M3B; Box_M3C; Box_M3D; Box_M3_Stair; Box_M3_StairB; Box_M4; Box_M4B; Box_M4C; Box_M4D; Box_M4_Stair; Box_M4_StairB; Box_M5; Box_M5B; Box_M5C; Box_M5D; Box_M5_Stair; Box_M5_StairB; Box_M6; Box_M6B; Box_M6C; Box_M6D; Box_M6_Stair; Box_M6_StairB; Box_M7; Box_M7B; Box_M7C; Box_M7D; Box_M7_Stair; Box_M7_StairB; Box_M8; Box_M8B; Box_M8C; Box_M8D; Box_M8_Stair; Box_M8_StairB; Box_N1; Box_N1B; Box_N1C; Box_N1D; Box_N2; Box_N2B; Box_N2C; Box_N2D; Box_N3; Box_N3B; Box_N3C; Box_N3D; Box_N4; Box_N4B; Box_N4C; Box_N4D; Box_N5; Box_N5B; Box_N5C; Box_N5D; Box_N6; Box_N6B; Box_N6C; Box_N6D; Box_N7; Box_N7B; Box_N7C; Box_N7D; Box_N8; Box_N8B; Box_N8C; Box_N8D; Box_O1; Box_O2; Box_O3; Box_O4; Box_O5; Box_O6; Box_O7; Box_O8; Box_Water_A; Box_Water_B; Box_Water_C; Box_Water_D; Buildings_A1; Buildings_A2; Buildings_B1; Buildings_B2; Buildings_B3; Buildings_C1; Buildings_C2; Buildings_C3; Buildings_D1; Buildings_D2; Buildings_E1; Buildings_E2; Buildings_E3; Buildings_F1; Buildings_F2; Buildings_F3; Buildings_G1; Buildings_G2; Buildings_H1; Buildings_H2; Buildings_I1; Buildings_I2; Buildings_I3; Buildings_I4; Buildings_I5; Buildings_I6; Buildings_I7; Buildings_J1_1; Buildings_J2_1; Buildings_J3_1; Buildings_K1; Buildings_K2; Buildings_K3; Buildings_L1; Buildings_L2; Buildings_L3; Buildings_L4; Buildings_L5; Buildings_L6; Buildings_L7; Interiors_Props_A1; Interiors_Props_A2; Interiors_Props_B1; Interiors_Props_B2; Interiors_Props_B3; Interiors_Props_C1; Interiors_Props_C2; Interiors_Props_D1; Interiors_Props_D2; Interiors_Props_D3; Interiors_Props_E1; Interiors_Props_E2; Interiors_Props_E3; Interiors_Props_F1; Interiors_Props_F2; Interiors_Props_G; Interiors_Props_H1; Interiors_Props_H2; Interiors_Props_I1; Interiors_Props_I2; Interiors_Props_I3; Interiors_Props_I4; Interiors_Props_J1; Interiors_Props_J2; Interiors_Props_K1; Interiors_Props_K2; Interiors_Props_K3; Interiors_Props_K4; Interiors_Props_L1; Interiors_Props_L2; Interiors_Props_L3; Interiors_Props_M; Interiors_Props_N; Interiors_Props_O1; Interiors_Props_O2; Interiors_Props_O3; Interiors_Props_O4; Interiors_Props_O5; Interiors_Props_P1; Interiors_Props_P2; Interiors_Props_Q1; Interiors_Props_Q2; Interiors_Props_Q3; Interiors_Props_R1; Interiors_Props_R2; Interiors_Props_R3; Interiors_Props_S1; Interiors_Props_S2; Interiors_Props_T1; Interiors_Props_T2; Interiors_Props_T3; Interiors_Props_U1; Interiors_Props_U2; Interiors_Props_U3; Interiors_Props_U4; Plants_Flower_A1; Plants_Flower_A2; Plants_Flower_A3; Plants_Flower_A4; Plants_Flower_A5; Plants_Flower_A6; Plants_Flower_A7; Plants_Flower_B1; Plants_Flower_B2; Plants_Flower_B3; Plants_Flower_B4; Plants_Flower_B5; Plants_Flower_B6; Plants_Flower_C1; Plants_Flower_C2; Plants_Flower_C3; Plants_Flower_C4; Plants_Flower_D1; Plants_Flower_D2; Plants_Flower_D3; Plants_Flower_D4; Plants_Flower_E1; Plants_Flower_E2; Plants_Flower_E3; Plants_Flower_E4; Plants_Flower_F1; Plants_Flower_F2; Plants_Flower_F3; Plants_Flower_F4; Plants_Flower_G1; Plants_Flower_G2; Plants_Flower_G3; Plants_Flower_G4; Plants_Grass_A1; Plants_Grass_A2; Plants_Grass_A3; Plants_Grass_B1; Plants_Grass_B2; Plants_Grass_C1; Plants_Grass_C2; Plants_Grass_C3; Plants_Grass_C4; Plants_Grass_D1; Plants_Grass_D2; Plants_Grass_D3; Plants_Grass_D4; Plants_Grass_E1; Plants_Grass_E2; Plants_Grass_E3; Plants_Grass_F1; Plants_Grass_F2; Plants_Grass_F3; Plants_Grass_G1; Plants_Grass_G2; Plants_Grass_G3; Plants_Grass_G4; Plants_Tree_A1; Plants_Tree_A2; Plants_Tree_A3; Plants_Tree_A4; Plants_Tree_A5; Plants_Tree_A6; Plants_Tree_A7; Plants_Tree_AB1; Plants_Tree_AB2; Plants_Tree_AB3; Plants_Tree_AB4; Plants_Tree_AB5; Plants_Tree_AB6; Plants_Tree_AB7; Plants_Tree_B1; Plants_Tree_B2; Plants_Tree_B3; Plants_Tree_B4; Plants_Tree_C1; Plants_Tree_C2; Plants_Tree_C3; Plants_Tree_C4; Plants_Tree_C5; Plants_Tree_D1; Plants_Tree_D2; Plants_Tree_D3; Plants_Tree_D4; Plants_Tree_D5; Plants_Tree_D6; Plants_Tree_E1; Plants_Tree_E2; Plants_Tree_E3; Plants_Tree_E4; Plants_Tree_F1; Plants_Tree_F2; Plants_Tree_F3; Plants_Tree_F4; Plants_Vines_A1; Plants_Vines_A2; Plants_Vines_A3; Plants_Vines_A4; Plants_Vines_A5; Plants_Vines_A6; Plants_Vines_A7; Plants_Vines_B1; Plants_Vines_B2; Plants_Vines_B3; Plants_Vines_B4; Plants_Vines_C1; Plants_Vines_C2; Plants_Vines_C3; Plants_Vines_C4; Plants_Vines_C5; Plants_Vines_C6; Props_A1; Props_A10; Props_A11; Props_A12; Props_A13; Props_A14; Props_A15; Props_A16; Props_A17; Props_A18; Props_A19; Props_A2; Props_A20; Props_A21; Props_A22; Props_A23; Props_A24; Props_A3; Props_A4; Props_A5; Props_A6; Props_A7; Props_A8; Props_A9; Props_B10_1; Props_B11_1; Props_B11_2; Props_B12_1; Props_B12_2; Props_B13; Props_B14_1; Props_B14_2; Props_B15; Props_B16_1; Props_B16_2; Props_B17_1; Props_B17_2; Props_B17_3; Props_B18_1; Props_B18_2; Props_B18_3; Props_B18_4; Props_B19_1; Props_B19_2; Props_B19_3; Props_B19_4; Props_B1_1; Props_B1_2; Props_B1_3; Props_B1_4; Props_B20_1; Props_B20_2; Props_B20_3; Props_B21_1; Props_B21_2; Props_B21_3; Props_B22_1; Props_B22_2; Props_B23; Props_B24; Props_B25; Props_B2_1; Props_B2_2; Props_B3_1; Props_B3_2; Props_B3_3; Props_B4_1; Props_B4_2; Props_B4_3; Props_B4_4; Props_B4_5; Props_B4_6; Props_B5_1; Props_B5_2; Props_B5_3; Props_B5_4; Props_B6_1; Props_B6_2; Props_B6_3; Props_B6_4; Props_B7_1; Props_B7_2; Props_B8_1; Props_B8_2; Props_B9_1; Props_B9_2; Props_C1; Props_C10_1; Props_C10_2; Props_C11_1; Props_C11_2; Props_C12_1; Props_C12_2; Props_C13_1; Props_C14_1; Props_C15; Props_C16; Props_C17; Props_C18; Props_C19_1; Props_C19_2; Props_C20_1; Props_C20_2; Props_C21_1; Props_C21_2; Props_C22_1; Props_C22_2; Props_C22_4; Props_C22_5; Props_C22_6; Props_C23_1; Props_C23_2; Props_C24_1; Props_C24_10; Props_C24_2; Props_C24_3; Props_C24_4; Props_C24_5; Props_C24_6; Props_C24_7; Props_C24_8; Props_C24_9; Props_C25; Props_C26; Props_C27; Props_C28; Props_C29_1; Props_C29_2; Props_C2_1; Props_C2_2; Props_C2_3; Props_C3; Props_C4_1; Props_C4_2; Props_C4_3; Props_C5_1; Props_C5_2; Props_C6_1; Props_C6_2; Props_C6_3; Props_C7; Props_C8_1; Props_C8_2; Props_C8_3; Props_C8_4; Props_C9_1; Props_C9_2; Props_C9_3; Props_Ice_1; Props_Ice_10; Props_Ice_11; Props_Ice_12; Props_Ice_13; Props_Ice_14; Props_Ice_2; Props_Ice_3; Props_Ice_4; Props_Ice_5; Props_Ice_6; Props_Ice_7; Props_Ice_8; Props_Ice_9; Rock_1; Rock_10; Rock_11; Rock_12; Rock_13; Rock_14; Rock_15; Rock_16; Rock_17; Rock_18; Rock_19; Rock_2; Rock_20; Rock_21; Rock_22; Rock_23; Rock_24; Rock_25; Rock_26; Rock_27; Rock_28; Rock_3; Rock_4; Rock_5; Rock_6; Rock_7; Rock_8; Rock_9; Rock_S_1; Rock_S_10; Rock_S_11; Rock_S_12; Rock_S_13; Rock_S_14; Rock_S_15; Rock_S_16; Rock_S_17; Rock_S_18; Rock_S_19; Rock_S_2; Rock_S_20; Rock_S_21; Rock_S_22; Rock_S_23; Rock_S_24; Rock_S_25; Rock_S_26; Rock_S_27; Rock_S_28; Rock_S_3; Rock_S_4; Rock_S_5; Rock_S_6; Rock_S_7; Rock_S_8; Rock_S_9; Tree_A1; Tree_A10; Tree_A11; Tree_A12; Tree_A13; Tree_A14; Tree_A15; Tree_A16; Tree_A17; Tree_A18; Tree_A19; Tree_A2; Tree_A20; Tree_A21; Tree_A3; Tree_A4; Tree_A5; Tree_A6; Tree_A7; Tree_A8; Tree_A9";
        boxname = boxname.Replace(" ", "");
        boxNames = boxname.Split(';');
        mapItemList = new GameObject[588];

        for (i = 0; i < boxNames.Length; i++)
        {
            mapItemList[i] = (GameObject)Resources.Load("map/Prefabs/" + boxNames[i]);
        }




        /*if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);


            for (i = 0; i < files.Length; i++)
            {
                if (!files[i].Name.EndsWith(".prefab"))
                {
                    continue;
                }
                string str = files[i].Name.Replace(".prefab", "");

                if (files[i].Name.IndexOf("Plants_Flower") > -1)
                {
                    if (flower.Length > 0)
                        flower += ";";
                    flower += str;
                }
                

                if (boxname.Length > 0)
                    boxname += ";";
                boxname += str;
            }
            
            boxNames = boxname.Split(';');
            flowerNames = flower.Split(';');
        }

        mapItemList = new GameObject[boxNames.Length];
        flowersList = new GameObject[flowerNames.Length];


        string boxNamess = "";
        for ( i = 0; i < boxNames.Length; i++)
        {
            boxNamess += boxNames[i] + ";";
            debugtxt.text = boxNames[i];
            mapItemList[i] = (GameObject)Resources.Load("map/Prefabs/" + boxNames[i]);
        }
        Debug.Log(boxNamess);
        for ( i = 0; i < flowerNames.Length; i++)
        {
            flowersList[i] = (GameObject)Resources.Load("map/Prefabs/" + flowerNames[i]);
        }*/
    }
}