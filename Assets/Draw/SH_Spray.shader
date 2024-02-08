Shader "Custom/SprayShader" {
    Properties{
        _Color("Main Color", Color) = (.5,.5,.5,1)
    }

        SubShader{
            Tags { "Queue" = "Overlay" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                ENDCG

                SetTexture[_MainTex] {
                    combine primary
                }
            }
    }
}