using System.Collections;
using UnityEngine;

// This is our namespace so we dont get mixups with assets
using Com.MyCompany.MyGame;

public class PropValues : MonoBehaviour
{
    // Use for dynamic list/array
    // using System.Collections.Generics;
    //      public List<int> arrayOfInts;
    // Ids are set to PropValue in PropHunt.cs
    #region Public Variables

    [Tooltip("IDs are indexed by their array's position. This array contains the Shared resource Mesh locations")]
    public string[] values;

    #endregion

    /* TABLE OF ALL TRANSFORMABLE PROPS
     * Currently, implementing the same logic as before is difficult with multiplayer
     * Hence, we must use a table to lookup prop transformations across all players.
    /*  Name                | Instance Name |       Path                |       ID
     * Prop_Axe             | Cube.146      | Mesh/Prop_Axe             |       1
     * Prop_Barrel          | Cylinder.121  | Mesh/Prop_Barrel          |       2
     * Prop_Crate           | Cube.144      | Mesh/Prop_Crate           |       3
     * Prop_Fence           | Cube.145      | Mesh/Prop_Fence           |       4
     * Prop_Mushroom        | Cube.149      | Mesh/Prop_Mushroom        |       5
     * Prop_Stone_1         | Icosphere.074 | Mesh/Prop_Stone_1         |       6
     * Prop_Stone_2         | Icosphere.072 | Mesh/Prop_Stone_2         |       7
     * Prop_Stone_3         | Icosphere.073 | Mesh/Prop_Stone_3         |       8
     * Prop_TreeTrunk_Cutoff| Cylinder.129  | Mesh/Prop_TreeTrunk_Cutoff|       9
     * Prop_Wood_1          | Cylinder.122  | Mesh/Prop_Wood_1          |       10
     * Prop_Wood_2          | Cylinder.126  | Mesh/Prop_Wood_2          |       11
     * Prop_WoodenWheelBarro| Cube.143      | Mesh/Prop_WoodenWheelBarrow|      12
     * Trees_Green          | Cylinder.118  | Mesh/Trees_Green          |       13
     * Trees_Pink           | Cylinder.119  | Mesh/Trees_Pink           |       14
     * Trees_Yellow         | Cylinder.120  | Mesh/Trees_Yellow         |       15
     * 
     * Use GameObject meshTest = Resources.Load<GameObject>("/Filepath");
     */

    // Start is called before the first frame update
    void Start()
    {
        values = new string[16];
        values[0] = "null";
        values[1] = "Mesh/Prop_Axe";
        values[2] = "Mesh/Prop_Barrel";
        values[3] = "Mesh/Prop_Crate";
        values[4] = "Mesh/Prop_Fence";
        values[5] = "Mesh/Prop_Mushroom";
        values[6] = "Mesh/Prop_Stone_1";
        values[7] = "Mesh/Prop_Stone_2";
        values[8] = "Mesh/Prop_Stone_3";
        values[9] = "Mesh/Prop_TreeTrunk_Cutoff";
        values[10] = "Mesh/Prop_Wood_1";
        values[11] = "Mesh/Prop_Wood_2";
        values[12] = "Mesh/Prop_WoodenWheelBarrow";
        values[13] = "Mesh/Trees_Green";
        values[14] = "Mesh/Trees_Pink";
        values[15] = "Mesh/Trees_Yellow";
        // Add to the table here as we go (Dont forget to increase array size-- or just use lists lol)
    }
}
