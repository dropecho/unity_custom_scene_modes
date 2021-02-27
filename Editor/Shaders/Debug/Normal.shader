Shader "Dropecho/Unlit/Normal Debug"
{
  SubShader
  {
    Pass
    {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      struct appdata
      {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
      };

      struct v2f
      {
        float4 vertex : SV_POSITION;
        float3 normal : NORMAL;
      };

      v2f vert (appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.normal = v.normal;
        return o;
      }

      fixed4 frag (v2f i) : SV_Target
      {
        return float4(i.normal, 1);
      }
      ENDCG
    }
  }
}
