Shader "Custom/DSNewShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _iRes("Bombing", Range(0, 2)) = 0.5
        _BlendWidth("Blend Width", Range(0, 2)) = 0.4

        [NoScaleOffset] _ParallaxMap ("Parallax", 2D) = "black" {}
		_ParallaxStrength ("Parallax Strength", Range(0, 0.1)) = 0

        [NoScaleOffset] _NormalMap ("Normals", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _PARALLAX_MAP

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            sampler2D _NormalMap;
            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed _Cutoff;
            float _iRes;
            float _BlendWidth;

            sampler2D _ParallaxMap;
            float _ParallaxStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 hash4( fixed2 p ) { return frac(sin(fixed4( 1.0+dot(p,fixed2(37.0,17.0)),
                                              2.0+dot(p,fixed2(11.0,47.0)),
                                              3.0+dot(p,fixed2(41.0,29.0)),
                                              4.0+dot(p,fixed2(23.0,31.0))))*103.0); }

            float2 transformUVs( in float2 iuvCorner, in float2 uv )
            {
                // random in [0,1]^4
                float4 tx = hash4( iuvCorner );
                // scale component is +/-1 to mirror
                tx.zw = sign( tx.zw - 0.5 );
                // debug vis
                #if JIGGLE
                tx.xy *= .05*sin(5.*iTime+iuvCorner.x+iuvCorner.y);
                #endif
                // random scale and offset
                return tx.zw * uv + tx.xy;
            }

            

            fixed4 textureNoTile_3weights( sampler2D samp, in float2 uv )
            {
                fixed4 res = (0,0,0,0);
                int sampleCnt = 0; // debug vis
                
                // compute per-tile integral and fractional uvs.
                // flip uvs for 'odd' tiles to make sure tex samples are coherent
                float2 fuv = fmod( uv, 2. ), iuv = uv - fuv;
                float3 BL_one = float3(0.,0.,1.); // xy = bot left coords, z = 1
                if( fuv.x >= 1. ) fuv.x = 2.-fuv.x, BL_one.x = 2.;
                if( fuv.y >= 1. ) fuv.y = 2.-fuv.y, BL_one.y = 2.;
                
                
                // weight orthogonal to diagonal edge = 3rd texture sample
                float2 iuv3;
                float w3 = (fuv.x+fuv.y) - 1.;
                if( w3 < 0. ) iuv3 = iuv + BL_one.xy, w3 = -w3; // bottom left corner, offset negative, weight needs to be negated
                else iuv3 = iuv + BL_one.zz; // use transform from top right corner
                w3 = smoothstep(_BlendWidth, 1.-_BlendWidth, w3);
                
                // if third sample doesnt dominate, take first two
                if( w3 < 0.999 )
                {
                    // use weight along long diagonal edge
                    float w12 = dot(fuv,float2(.5,-.5)) + .5;
                    w12 = smoothstep(1.125*_BlendWidth, 1.-1.125*_BlendWidth, w12);

                    // take samples from texture for each side of diagonal edge
                    if( w12 > 0.001 ) res +=     w12  * tex2D( samp, transformUVs( iuv + BL_one.zy, uv ) ), sampleCnt++;
                    if( w12 < 0.999 ) res += (1.-w12) * tex2D( samp, transformUVs( iuv + BL_one.xz, uv ) ), sampleCnt++;
                }
                
                // first two samples aren't dominating, take third
                if( w3 > 0.001 ) res = lerp( res, tex2D( samp, transformUVs( iuv3, uv ) ), w3 ), sampleCnt++;

                
                // debug vis: colour based on num samples taken for vis purposes
                
                return res;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv / _iRes;
                

                fixed4 fragColor = textureNoTile_3weights( _MainTex, uv );
                return fragColor;
            }
            ENDCG
        }
    }
}
