

//
// e.g. SmoothstepMountain_float(x, 0.8, 0.4, 0.15, y);
//
void SmoothstepMountain_float(float x,
    float centerX, float mainWidth, float slopeWidth,
    out float y)
{
    float halfMainWidth = mainWidth / 2.0;
    float begin = centerX - halfMainWidth;
    float end = centerX + halfMainWidth;
    
    y = smoothstep(begin, begin + slopeWidth, x)
        * smoothstep(end, end - slopeWidth, x);
}