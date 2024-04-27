#ifndef GETSHADOWLEVEL_HLSL
#define GETSHADOWLEVEL_HLSL

uniform float _PlayerShadowLevel = 0;

void PlayerShadowLevel_float(out float1 Out){

    Out = _PlayerShadowLevel;
}
#endif