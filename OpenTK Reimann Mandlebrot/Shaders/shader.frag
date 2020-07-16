#version 450 core

#define PI 3.1415926535897932384626433832795
#define MAX_MATING_ITER 25

// Only works with GLSL 4 and above
const float infinity = 1.0 / 0.0;

in vec3 Normal;             // The normal of the fragment is calculated in the vertex shader.
in vec3 FragPosWorld;       // The fragment position in world space.
in vec3 FragPosModel;       // The fragment position in model space.
in vec2 TexCoord;           // 

out vec4 FragColor;

// I couldn't figure out how to SUCCESSFULLY send a uniform array (all my attempts only sent the first element)
uniform vec2 ma0;
uniform vec2 ma1;
uniform vec2 ma2;
uniform vec2 ma3;
uniform vec2 ma4;
uniform vec2 ma5;
uniform vec2 ma6;
uniform vec2 ma7;
uniform vec2 ma8;
uniform vec2 ma9;
uniform vec2 ma10;
uniform vec2 ma11;
uniform vec2 ma12;
uniform vec2 ma13;
uniform vec2 ma14;
uniform vec2 ma15;
uniform vec2 ma16;
uniform vec2 ma17;
uniform vec2 ma18;
uniform vec2 ma19;
uniform vec2 ma20;
uniform vec2 ma21;
uniform vec2 ma22;
uniform vec2 ma23;
uniform vec2 ma24;
uniform vec2 mb0;
uniform vec2 mb1;
uniform vec2 mb2;
uniform vec2 mb3;
uniform vec2 mb4;
uniform vec2 mb5;
uniform vec2 mb6;
uniform vec2 mb7;
uniform vec2 mb8;
uniform vec2 mb9;
uniform vec2 mb10;
uniform vec2 mb11;
uniform vec2 mb12;
uniform vec2 mb13;
uniform vec2 mb14;
uniform vec2 mb15;
uniform vec2 mb16;
uniform vec2 mb17;
uniform vec2 mb18;
uniform vec2 mb19;
uniform vec2 mb20;
uniform vec2 mb21;
uniform vec2 mb22;
uniform vec2 mb23;
uniform vec2 mb24;
uniform vec2 mc0;
uniform vec2 mc1;
uniform vec2 mc2;
uniform vec2 mc3;
uniform vec2 mc4;
uniform vec2 mc5;
uniform vec2 mc6;
uniform vec2 mc7;
uniform vec2 mc8;
uniform vec2 mc9;
uniform vec2 mc10;
uniform vec2 mc11;
uniform vec2 mc12;
uniform vec2 mc13;
uniform vec2 mc14;
uniform vec2 mc15;
uniform vec2 mc16;
uniform vec2 mc17;
uniform vec2 mc18;
uniform vec2 mc19;
uniform vec2 mc20;
uniform vec2 mc21;
uniform vec2 mc22;
uniform vec2 mc23;
uniform vec2 mc24;
uniform vec2 md0;
uniform vec2 md1;
uniform vec2 md2;
uniform vec2 md3;
uniform vec2 md4;
uniform vec2 md5;
uniform vec2 md6;
uniform vec2 md7;
uniform vec2 md8;
uniform vec2 md9;
uniform vec2 md10;
uniform vec2 md11;
uniform vec2 md12;
uniform vec2 md13;
uniform vec2 md14;
uniform vec2 md15;
uniform vec2 md16;
uniform vec2 md17;
uniform vec2 md18;
uniform vec2 md19;
uniform vec2 md20;
uniform vec2 md21;
uniform vec2 md22;
uniform vec2 md23;
uniform vec2 md24;





uniform int maxIterations;
uniform float bailout;
uniform int intermediateSteps;
uniform int currentMatingIteration;
uniform vec2 p;
uniform vec2 q;
uniform double R_t;


uniform float zoom;
uniform float time;
uniform float rPos;
uniform float iPos;

vec2 ma[MAX_MATING_ITER];
vec2 mb[MAX_MATING_ITER];
vec2 mc[MAX_MATING_ITER];
vec2 md[MAX_MATING_ITER];

vec3 Riemann();

void main()
{
    ma[0] = ma0;
    ma[1] = ma1;
    ma[2] = ma2;
    ma[3] = ma3;
    ma[4] = ma4;
    ma[5] = ma5;
    ma[6] = ma6;
    ma[7] = ma7;
    ma[8] = ma8;
    ma[9] = ma9;
    ma[10] = ma10;
    ma[11] = ma11;
    ma[12] = ma12;
    ma[13] = ma13;
    ma[14] = ma14;
    ma[15] = ma15;
    ma[16] = ma16;
    ma[17] = ma17;
    ma[18] = ma18;
    ma[19] = ma19;
    ma[20] = ma20;
    ma[21] = ma21;
    ma[22] = ma22;
    ma[23] = ma23;
    ma[24] = ma24;
    mb[0] = mb0;
    mb[1] = mb1;
    mb[2] = mb2;
    mb[3] = mb3;
    mb[4] = mb4;
    mb[5] = mb5;
    mb[6] = mb6;
    mb[7] = mb7;
    mb[8] = mb8;
    mb[9] = mb9;
    mb[10] = mb10;
    mb[11] = mb11;
    mb[12] = mb12;
    mb[13] = mb13;
    mb[14] = mb14;
    mb[15] = mb15;
    mb[16] = mb16;
    mb[17] = mb17;
    mb[18] = mb18;
    mb[19] = mb19;
    mb[20] = mb20;
    mb[21] = mb21;
    mb[22] = mb22;
    mb[23] = mb23;
    mb[24] = mb24;
    mc[0] = mc0;
    mc[1] = mc1;
    mc[2] = mc2;
    mc[3] = mc3;
    mc[4] = mc4;
    mc[5] = mc5;
    mc[6] = mc6;
    mc[7] = mc7;
    mc[8] = mc8;
    mc[9] = mc9;
    mc[10] = mc10;
    mc[11] = mc11;
    mc[12] = mc12;
    mc[13] = mc13;
    mc[14] = mc14;
    mc[15] = mc15;
    mc[16] = mc16;
    mc[17] = mc17;
    mc[18] = mc18;
    mc[19] = mc19;
    mc[20] = mc20;
    mc[21] = mc21;
    mc[22] = mc22;
    mc[23] = mc23;
    mc[24] = mc24;
    md[0] = md0;
    md[1] = md1;
    md[2] = md2;
    md[3] = md3;
    md[4] = md4;
    md[5] = md5;
    md[6] = md6;
    md[7] = md7;
    md[8] = md8;
    md[9] = md9;
    md[10] = md10;
    md[11] = md11;
    md[12] = md12;
    md[13] = md13;
    md[14] = md14;
    md[15] = md15;
    md[16] = md16;
    md[17] = md17;
    md[18] = md18;
    md[19] = md19;
    md[20] = md20;
    md[21] = md21;
    md[22] = md22;
    md[23] = md23;
    md[24] = md24;


    FragColor = vec4(Riemann(), 1.0);
}

dvec2 dc_conj(dvec2 c)
{
    return dvec2(c.x, -c.y);
}

dvec2 dc_2(dvec2 c)
{
    return vec2(c.x*c.x - c.y*c.y, 2*c.x*c.y);
}

dvec2 dc_mult(dvec2 a, dvec2 b)
{
    return vec2(a.x*b.x - a.y*b.y, a.x*b.y + a.y*b.x);
}

double dc_conj_mult(dvec2 c)
{
    return dc_mult(c, dc_conj(c)).x;
}

dvec2 dc_div(dvec2 a, dvec2 b)
{/*
    return c_mult(a, c_conj(b)) / c_conj_mult(b);
    */
	double x = b.x * b.x + b.y * b.y;
	return vec2((a.x * b.x + a.y * b.y) / x, (b.x * a.y - a.x * b.y) / x);
    
}

dvec2 dc_sqrt(dvec2 c)
{
    double r = length(c);
    double a = 1/sqrt(2);
    
    //return vec2(a * sqrt(r + c.x), sign(c.y) * a * sqrt(r - c.x));
    return .5 * sqrt(2) * vec2(sqrt(r + c.x), sign(c.y) * a * sqrt(r - c.x));
}

dvec2 dcproj(dvec2 c)
{
    if (!isinf(c.x) && !isinf(c.y) && !isnan(c.x) && !isnan(c.y))
        return c;
        
    return vec2(infinity, 0);
    //return vec2(infinity, sign(c.y) * 1e-10);
}

vec2 c_conj(vec2 c)
{
    return vec2(c.x, -c.y);
}

vec2 c_2(vec2 c)
{
    return vec2(c.x*c.x - c.y*c.y, 2*c.x*c.y);
}

vec2 c_mult(vec2 a, vec2 b)
{
    return vec2(a.x*b.x - a.y*b.y, a.x*b.y + a.y*b.x);
}

float c_conj_mult(vec2 c)
{
    return c_mult(c, c_conj(c)).x;
}

vec2 c_div(vec2 a, vec2 b)
{/*
    return c_mult(a, c_conj(b)) / c_conj_mult(b);
    */
	double x = b.x * b.x + b.y * b.y;
	return vec2((a.x * b.x + a.y * b.y) / x, (b.x * a.y - a.x * b.y) / x);
    
}

vec2 c_sqrt(vec2 c)
{
    float r = length(c);
    float a = 1/sqrt(2);
    
    //return vec2(a * sqrt(r + c.x), sign(c.y) * a * sqrt(r - c.x));
    return .5 * sqrt(2) * vec2(sqrt(r + c.x), sign(c.y) * a * sqrt(r - c.x));
}

vec2 cproj(vec2 c)
{
    if (!isinf(c.x) && !isinf(c.y) && !isnan(c.x) && !isnan(c.y))
        return c;
        
    return vec2(infinity, 0);
    //return vec2(infinity, sign(c.y) * 1e-10);
}

vec3 ColorFromHSV(vec3 color)
{
    float hi = mod(floor(color.x / 60.0), 6);
    float f = color.x / 60.0 - floor(color.x / 60.0);

    //value = value * 255;
    float v = color.z;
    float p = color.z * (1 - color.y);
    float q = color.z * (1 - f * color.y);
    float t = color.z * (1 - (1 - f) * color.y);

    if (hi == 0)
        return vec3(v, t, p);
    if (hi == 1)
        return vec3(q, v, p);
    if (hi == 2)
        return vec3(p, v, t);
    if (hi == 3)
        return vec3(p, q, v);
    if (hi == 4)
        return vec3(t, p, v);

    return vec3(v, p, q);
}

vec3 JuliaMatingLoop(dvec2 z)
{
    // julia
    vec2 c;
    // julia iterated point
    vec2 w;
    // output color
    vec3 color;
    
    // orbit push forward
    for (int k = currentMatingIteration; k >= 0; --k)
    {
        z = dcproj(dc_2(z));
        z = dcproj(dc_div(dc_mult(ma[k], z) + mb[k], dc_mult(mc[k], z) + md[k]));
    }

    // Decide which hemisphere we're in
    if (length(z) < 1)
    {
        color = vec3(.8);    // light gray
        c = p;
        w = vec2(R_t * z);
    }
    else
    {
        color = vec3(.2);    // dark grey
        c = q;
        
        if (abs(z.y) < 1e-7)    // reduces error
            w = vec2(R_t / z.x, 0);
        else
            w = vec2(dcproj(dc_div(dvec2(R_t,0), z)));
    }


    int iter = 0;
    for (iter = currentMatingIteration + 1; iter < maxIterations && (w.x * w.x + w.y * w.y < bailout); iter++)
        w = c_2(w) + c;

    
    // coloring
    if (iter >= maxIterations)
    {
        //color = pow(normalize(FragPosModel), vec3(.9));
        color = mix(1 - pow(normalize(FragPosModel), vec3(.9)), vec3(sin(1 * time / 11), sin(2 * time / 13), sin(3 * time / 17)), color.x);
    }
    else
    {
        /*
        int newMax = iter + 3;

        // Smooth iteration
        for (iter = iter; iter < newMax; iter++)
            w = c_2(w) + c;
        
        float mu = iter + 1 - (log2(log2(length(w))));
        float t = time * -5;
        vec3 color = vec3(sin(7 * (mu+t/2) / 17), sin(11 * (mu+t/3) / 29), sin(13 * (mu+t/5) / 41));
        */
    }

    return color;
}

vec3 Riemann()
{
    // Riemann projection
    vec3 pos = normalize(vec3(FragPosModel.x, FragPosModel.y, FragPosModel.z));

    float tmp = (1 + (pos.y + 1)/(1 - pos.y)) / 2.0 / pow(2,zoom);
    float r = pos.x*tmp;
    float i = pos.z*tmp;
    
    vec2 z = vec2(r + rPos, i + iPos);

    return JuliaMatingLoop(z);
}
