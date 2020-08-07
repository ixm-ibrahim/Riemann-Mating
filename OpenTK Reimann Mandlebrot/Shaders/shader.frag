﻿#version 450 core

#define MAX_FLOAT 3.402823466e+18
#define PI 3.1415926535897932384626433832795
#define MAX_MATING_ITER 50

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
uniform vec2 ma25;
uniform vec2 ma26;
uniform vec2 ma27;
uniform vec2 ma28;
uniform vec2 ma29;
uniform vec2 ma30;
uniform vec2 ma31;
uniform vec2 ma32;
uniform vec2 ma33;
uniform vec2 ma34;
uniform vec2 ma35;
uniform vec2 ma36;
uniform vec2 ma37;
uniform vec2 ma38;
uniform vec2 ma39;
uniform vec2 ma40;
uniform vec2 ma41;
uniform vec2 ma42;
uniform vec2 ma43;
uniform vec2 ma44;
uniform vec2 ma45;
uniform vec2 ma46;
uniform vec2 ma47;
uniform vec2 ma48;
uniform vec2 ma49;
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
uniform vec2 mb25;
uniform vec2 mb26;
uniform vec2 mb27;
uniform vec2 mb28;
uniform vec2 mb29;
uniform vec2 mb30;
uniform vec2 mb31;
uniform vec2 mb32;
uniform vec2 mb33;
uniform vec2 mb34;
uniform vec2 mb35;
uniform vec2 mb36;
uniform vec2 mb37;
uniform vec2 mb38;
uniform vec2 mb39;
uniform vec2 mb40;
uniform vec2 mb41;
uniform vec2 mb42;
uniform vec2 mb43;
uniform vec2 mb44;
uniform vec2 mb45;
uniform vec2 mb46;
uniform vec2 mb47;
uniform vec2 mb48;
uniform vec2 mb49;
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
uniform vec2 mc25;
uniform vec2 mc26;
uniform vec2 mc27;
uniform vec2 mc28;
uniform vec2 mc29;
uniform vec2 mc30;
uniform vec2 mc31;
uniform vec2 mc32;
uniform vec2 mc33;
uniform vec2 mc34;
uniform vec2 mc35;
uniform vec2 mc36;
uniform vec2 mc37;
uniform vec2 mc38;
uniform vec2 mc39;
uniform vec2 mc40;
uniform vec2 mc41;
uniform vec2 mc42;
uniform vec2 mc43;
uniform vec2 mc44;
uniform vec2 mc45;
uniform vec2 mc46;
uniform vec2 mc47;
uniform vec2 mc48;
uniform vec2 mc49;
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
uniform vec2 md25;
uniform vec2 md26;
uniform vec2 md27;
uniform vec2 md28;
uniform vec2 md29;
uniform vec2 md30;
uniform vec2 md31;
uniform vec2 md32;
uniform vec2 md33;
uniform vec2 md34;
uniform vec2 md35;
uniform vec2 md36;
uniform vec2 md37;
uniform vec2 md38;
uniform vec2 md39;
uniform vec2 md40;
uniform vec2 md41;
uniform vec2 md42;
uniform vec2 md43;
uniform vec2 md44;
uniform vec2 md45;
uniform vec2 md46;
uniform vec2 md47;
uniform vec2 md48;
uniform vec2 md49;





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
    ma[25] = ma25;
    ma[26] = ma26;
    ma[27] = ma27;
    ma[28] = ma28;
    ma[29] = ma29;
    ma[30] = ma30;
    ma[31] = ma31;
    ma[32] = ma32;
    ma[33] = ma33;
    ma[34] = ma34;
    ma[35] = ma35;
    ma[36] = ma36;
    ma[37] = ma37;
    ma[38] = ma38;
    ma[39] = ma39;
    ma[40] = ma40;
    ma[41] = ma41;
    ma[42] = ma42;
    ma[43] = ma43;
    ma[44] = ma44;
    ma[45] = ma45;
    ma[46] = ma46;
    ma[47] = ma47;
    ma[48] = ma48;
    ma[49] = ma49;
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
    mb[25] = mb25;
    mb[26] = mb26;
    mb[27] = mb27;
    mb[28] = mb28;
    mb[29] = mb29;
    mb[30] = mb30;
    mb[31] = mb31;
    mb[32] = mb32;
    mb[33] = mb33;
    mb[34] = mb34;
    mb[35] = mb35;
    mb[36] = mb36;
    mb[37] = mb37;
    mb[38] = mb38;
    mb[39] = mb39;
    mb[40] = mb40;
    mb[41] = mb41;
    mb[42] = mb42;
    mb[43] = mb43;
    mb[44] = mb44;
    mb[45] = mb45;
    mb[46] = mb46;
    mb[47] = mb47;
    mb[48] = mb48;
    mb[49] = mb49;
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
    mc[25] = mc25;
    mc[26] = mc26;
    mc[27] = mc27;
    mc[28] = mc28;
    mc[29] = mc29;
    mc[30] = mc30;
    mc[31] = mc31;
    mc[32] = mc32;
    mc[33] = mc33;
    mc[34] = mc34;
    mc[35] = mc35;
    mc[36] = mc36;
    mc[37] = mc37;
    mc[38] = mc38;
    mc[39] = mc39;
    mc[40] = mc40;
    mc[41] = mc41;
    mc[42] = mc42;
    mc[43] = mc43;
    mc[44] = mc44;
    mc[45] = mc45;
    mc[46] = mc46;
    mc[47] = mc47;
    mc[48] = mc48;
    mc[49] = mc49;
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
    md[25] = md25;
    md[26] = md26;
    md[27] = md27;
    md[28] = md28;
    md[29] = md29;
    md[30] = md30;
    md[31] = md31;
    md[32] = md32;
    md[33] = md33;
    md[34] = md34;
    md[35] = md35;
    md[36] = md36;
    md[37] = md37;
    md[38] = md38;
    md[39] = md39;
    md[40] = md40;
    md[41] = md41;
    md[42] = md42;
    md[43] = md43;
    md[44] = md44;
    md[45] = md45;
    md[46] = md46;
    md[47] = md47;
    md[48] = md48;
    md[49] = md49;


    FragColor = vec4(Riemann(), 1.0);
}

dvec2 dc_conj(dvec2 c)
{
    return dvec2(c.x, -c.y);
}

dvec2 dc_2(dvec2 c)
{
    return dvec2(c.x*c.x - c.y*c.y, 2*c.x*c.y);
}

dvec2 dc_mult(dvec2 a, dvec2 b)
{
    return dvec2(a.x*b.x - a.y*b.y, a.x*b.y + a.y*b.x);
}

double dc_conj_mult(dvec2 c)
{
    return dc_mult(c, dc_conj(c)).x;
}

dvec2 dc_div(dvec2 a, dvec2 b)
{
    return dc_mult(a, dc_conj(b)) / dc_conj_mult(b);
    /*
	double x = b.x * b.x + b.y * b.y;
	return vec2((a.x * b.x + a.y * b.y) / x, (b.x * a.y - a.x * b.y) / x);
    */
}

dvec2 dc_sqrt(dvec2 c)
{
    double r = length(c);
    double a = 1/sqrt(2);
    
    //return dvec2(a * sqrt(r + c.x), sign(c.y) * a * sqrt(r - c.x));
    return .5 * sqrt(2) * dvec2(sqrt(r + c.x), sign(c.y) * a * sqrt(r - c.x));
}

dvec2 dcproj(dvec2 c)
{
    if (!isinf(c.x) && !isinf(c.y) && !isnan(c.x) && !isnan(c.y))
        return c;
        
    return dvec2(infinity, 0);
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
    if (length(z) <= 1)
    {
        color = vec3(.8);    // light gray
        c = vec2(p);
        w = vec2(R_t * z);
    }
    else
    {
        color = vec3(.2);    // dark grey
        c = vec2(q);
        
        if (abs(z.y) < 1e-7)    // reduces error
            w = vec2(R_t / z.x, 0);
        else
            w = cproj(vec2(dc_div(dvec2(R_t,0), z)));
            //w = vec2(dcproj(dc_div(dvec2(R_t,0), z)));
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
        int newIter = iter;
        for (newIter = iter; newIter < (iter + 3) && (w.x * w.x + w.y * w.y < bailout); newIter++)
            w = c_2(w) + c;
        
        float mu;

        if (isinf(length(w)))
            mu = iter + 1 - log2(log2(MAX_FLOAT));
            //mu = 0;
        else
            mu = iter + 1 - log2(log2(length(w)));

        float t = time * -5;

        vec3 muColor = vec3(sin(7 * (mu+t/2) / 17), sin(11 * (mu+t/3) / 29), sin(13 * (mu+t/5) / 41));
        
        //color = vec3(1);
        //color = muColor;
        color = 1 - muColor;
        
    }

    return color;
}

vec3 Riemann()
{
    // Riemann projection
    vec3 pos = normalize(vec3(FragPosModel.x, FragPosModel.y, FragPosModel.z));

    double tmp = (1 + (pos.y + 1)/(1 - pos.y)) / 2.0 / pow(2,zoom);
    double r = pos.x*tmp;
    double i = pos.z*tmp;
    
    dvec2 z = vec2(r + rPos, i + iPos);

    return JuliaMatingLoop(z);
}
