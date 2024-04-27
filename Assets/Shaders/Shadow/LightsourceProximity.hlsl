
#ifndef LIGHTSOURCEPROXIMITY_HLSL
#define LIGHTSOURCEPROXIMITY_HLSL

uniform float _Ranges[100];
uniform float3 _Positions[100];
uniform float _PositionArray;

void Proximity_float(float3 inputPosition, out float1 Out){
    float3 closestSource = _Positions[0];
    float1 chosenRange = _Ranges[0];

    Out = 10;

    for(int i = 0; i < _PositionArray; i++){
        float1 newBrightness = distance(inputPosition, _Positions[i]) - _Ranges[i];
        if(Out > newBrightness){
            Out = newBrightness;
        }
    }
}
#endif