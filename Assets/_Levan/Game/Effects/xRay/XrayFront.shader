Shader "GUI/XrayFront" 
{

    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _silhouetteColor ("silhouette Color", Color) = (0,1,0,1)
    }

    SubShader {
        Tags { "Queue"="Transparent-1" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting On Cull back ZWrite Off Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass 
        {
            // Only render texture & color into pixels when value in the stencil buffer is not equal to 1.
            Stencil 
            {
                Ref 1
                Comp NotEqual
            }
            ColorMaterial AmbientAndDiffuse
            SetTexture [_MainTex] 
            {
                constantColor [_Color]
                combine texture * constant
            }
        }

        Pass
        {
            // pixels whose value in the stencil buffer equals 1 should be drawn as _silhouetteColor
            Stencil {
              Ref 1
              Comp Equal
            }
            SetTexture [_MainTex] 
            {
                constantColor [_silhouetteColor]
                combine constant
            }

        }
    }
}