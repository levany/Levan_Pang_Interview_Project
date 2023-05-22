Shader "Custom/XrayBack" { //also used for silhouetted objects
Properties {}
SubShader {
    // Write the value 1 to the stencil buffer
    Stencil
    {
        Ref 1
        Comp Always
        Pass Replace
    }
    Tags { "Queue" = "Geometry-1" }  // Write to the stencil buffer before drawing any geometry to the screen
    ColorMask 0 // Don't write to any colour channels
    ZWrite Off // Don't write to the Depth buffer
    Pass {}
} 
FallBack "Diffuse"
}