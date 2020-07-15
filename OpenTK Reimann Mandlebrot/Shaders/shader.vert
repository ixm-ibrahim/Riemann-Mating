#version 450 core

/* gl_Position: built-in variable for vertex shaders that represents the final position of that vertex
 *      -Type: vec4
 */

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

out vec3 Normal;
out vec3 FragPosWorld;
out vec3 FragPosModel;
out vec2 TexCoord;
out vec3 MandelbrotColor;

uniform vec3 cPos;
uniform vec3 pCenter;
uniform float sideLength;
uniform float n;
uniform float minR;
uniform float maxR;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void Offset(inout vec3 pos, float zs);
vec3 SphericalProjection(inout vec3 pos, float radius);
void HemisphereApproximation(inout vec3 pos, vec3 c, float sideLength, float n);
void PlanetGround(inout vec3 pos, inout vec3 norm);

void main(void)
{
    vec3 pos = aPosition;
    vec3 norm = aNormal;

    PlanetGround(pos, norm);
    
    FragPosWorld = vec3(vec4(pos, 1.0) * model);
    FragPosModel = pos;
    
    gl_Position = vec4(pos, 1.0) * model * view * projection;

    Normal = norm * mat3(transpose(inverse(model)));
    
    TexCoord = aTexCoord;
}

// https://cescg.org/wp-content/uploads/2018/04/Michelic-Real-Time-Rendering-of-Procedurally-Generated-Planets-2.pdf
void PlanetGround(inout vec3 pos, inout vec3 norm)
{
    vec3 c = cPos / 1;
    

    pos /= sideLength / 2;
    HemisphereApproximation(pos, c, sideLength, n);
    pos*= sideLength / 2;

    float d = length(c - pCenter);
    float h = sqrt(d * d - minR * minR);
    float s = sqrt(maxR * maxR - minR * minR);
    float zs = (maxR * maxR + d * d - (h + s) * (h + s)) / (2 * minR * (h + s));

    Offset(pos, zs);

    vec3 b = normalize(vec3(1,1,1));
    vec3 w = normalize(c);
    vec3 v = -normalize(cross(w,b));
    vec3 u = -cross(w,v);
    mat3 r = mat3(u, v, w);
    
    pos = r * pos;
    norm = normalize(pos);

    norm = SphericalProjection(pos, minR);
    

}

void HemisphereApproximation(inout vec3 pos, vec3 c, float sideLength, float n)
{
    vec3 p = c - pos;

    pos.z = (1 - pow(pos.x, n)) * (1 - pow(pos.y, n));
                
}
 
void Offset(inout vec3 pos, float zs)
{
    //pos += pos*zs;
    pos.z += zs;
}

vec3 SphericalProjection(inout vec3 pos, float radius)
{
    vec3 norm = normalize(pos);

    pos = norm * radius;

    return norm;
}