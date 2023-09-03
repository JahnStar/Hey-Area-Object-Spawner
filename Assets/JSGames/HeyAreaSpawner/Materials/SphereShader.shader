Shader "Hey Games/Color Shader"{
    Properties{
        [HDR]_Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _ZOffsetFactor("Depth Offset Factor", Float) = 0
    }
    Category{
        Tags {
            "SplatCount" = "3"
            "Queue" = "Transparent"
            "RenderType" = "TransparentCutout"
            "ForceNoShadowCasting" = "True"
        }
        LOD 200
        Lighting Off
        ZWrite On
        Cull Back
        Offset[_ZOffsetFactor],0
        SubShader {
            Pass {
                SetTexture[_MainTex] {
                    constantColor[_Color]
                    Combine texture * constant, texture * constant
                }
            }
        }
    }
}