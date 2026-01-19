using UnityEngine;

public static class Animations
{
    public static Vector2 EaseInBounce(Vector2 start, Vector2 end, float val){
        return new Vector2(EaseInBounce(start.x, end.x, val), EaseInBounce(start.y, end.y, val));
    }

    public static Vector2 EaseOutBounce(Vector2 start, Vector2 end, float val){
        return new Vector2(EaseOutBounce(start.x, end.x, val), EaseOutBounce(start.y, end.y, val));
    }

    public static float EaseInBounce(float start, float end, float val){
        end -= start;
        float d = 1f;
        return end - EaseOutBounce(0, end, d-val) + start;
    }

    public static float EaseOutBounce(float start, float end, float val){
        val /= 1f;
        end -= start;
        if (val < (1 / 2.75f)){
            return end * (7.5625f * val * val) + start;
        }else if (val < (2 / 2.75f)){
            val -= (1.5f / 2.75f);
            return end * (7.5625f * (val) * val + .75f) + start;
        }else if (val < (2.5 / 2.75)){
            val -= (2.25f / 2.75f);
            return end * (7.5625f * (val) * val + .9375f) + start;
        }else{
            val -= (2.625f / 2.75f);
            return end * (7.5625f * (val) * val + .984375f) + start;
        }
    }
}
