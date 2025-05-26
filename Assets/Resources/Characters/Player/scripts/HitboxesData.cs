using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitboxesData", menuName = "Characters/Player/HitboxesData")]
public class HitboxesData : ScriptableObject
{
    public int nFrames = 2;
    
    public HitboxFrames down = new HitboxFrames()
    {
        frames = new List<HitboxFrame>()
        {
            // Frame 1
            new HitboxFrame() {
                points = new Vector2[]
                {
                    new Vector2(-0.1157372f, -0.9881545f),
                    new Vector2(-0.8026224f, -0.6901297f),
                    new Vector2(-1.159366f, -0.1930048f),
                    new Vector2(-1.167063f, 0.2461317f),
                    new Vector2(-1.016096f, 0.5320464f),
                    new Vector2(-1.186431f, 0.4457368f),
                    new Vector2(-1.365973f, -0.01749259f),
                    new Vector2(-1.380759f, -0.4327469f),
                    new Vector2(-1.166848f, -0.8910426f),
                    new Vector2(-0.7281567f, -1.287511f),
                    new Vector2(-0.1816834f, -1.497636f),
                    new Vector2(0.5525357f, -1.489088f),
                    new Vector2(0.786998f, -1.358569f),
                    new Vector2(0.4289818f, -0.9893f)
                }
            }
        }
    };

    public HitboxFrames up = new HitboxFrames()
    {
        frames = new List<HitboxFrame>()
        {
            // Frame 1
            new HitboxFrame() {
                points = new Vector2[]
                {
                    new Vector2(1.191553f, -0.7778684f),
                    new Vector2(1.244585f, -0.5926727f),
                    new Vector2(1.253297f, -0.02857262f),
                    new Vector2(0.9447165f, 0.6244807f),
                    new Vector2(0.627228f, 0.9891599f),
                    new Vector2(0.1821738f, 1.17738f),
                    new Vector2(-0.351351f, 1.189217f),
                    new Vector2(-0.8427372f, 0.9824725f),
                    new Vector2(-1.065668f, 0.6800967f),
                    new Vector2(-1.056526f, 0.3905157f),
                    new Vector2(-0.765165f, 0.1210283f),
                    new Vector2(-0.6922257f, 0.1881451f),
                    new Vector2(-0.6667982f, 0.3241816f),
                    new Vector2(-0.4524632f, 0.4748183f),
                    new Vector2(-0.08329308f, 0.5725002f),
                    new Vector2(0.4938895f, 0.5026314f),
                    new Vector2(0.8929617f, 0.2088158f),
                    new Vector2(1.114472f, -0.216605f),
                    new Vector2(1.189324f, -0.4085665f),
                    new Vector2(1.128803f, -0.5544964f),
                    new Vector2(1.070908f, -0.9278474f)
                }
            }
        }
    };

    public HitboxFrames left = new HitboxFrames()
    {
        frames = new List<HitboxFrame>()
        {
            new HitboxFrame() {
                points = new Vector2[]
                {
                    new Vector2(-1.277709f, 0.01080579f),
                    new Vector2(-1.60365f, -0.02758381f),
                    new Vector2(-1.75486f, -0.1696634f),
                    new Vector2(-1.768463f, -0.4326268f),
                    new Vector2(-1.489265f, -0.6079384f),
                    new Vector2(-0.5122218f, -0.637282f),
                    new Vector2(0.2217851f, -0.36825f),
                    new Vector2(0.6386701f, -0.04171109f),
                    new Vector2(-0.01556158f, -0.345214f),
                    new Vector2(-0.7517483f, -0.3556363f),
                    new Vector2(-0.9792457f, -0.1237553f)
                }
            }
        }
    };

    public HitboxFrames right = new HitboxFrames()
    {
        frames = new List<HitboxFrame>()
        {
            new HitboxFrame() {
                points = new Vector2[]
                {
                    new Vector2(1.749812f, -0.4192787f),
                    new Vector2(1.755852f, -0.2073484f),
                    new Vector2(1.552485f, 0.003936529f),
                    new Vector2(1.226544f, -0.006975561f),
                    new Vector2(0.939543f, -0.1338472f),
                    new Vector2(0.7645462f, -0.3636302f),
                    new Vector2(0.1937424f, -0.3685758f),
                    new Vector2(-0.6011389f, -0.02574861f),
                    new Vector2(-0.3748336f, -0.3075507f),
                    new Vector2(0.5328301f, -0.6304125f),
                    new Vector2(1.521217f, -0.632091f)
                }
            }
        }
    };
}