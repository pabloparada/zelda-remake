using System.Collections.Generic;

namespace LegendOfZelda.Animations
{
    public class Animation
    {
        public string               name;
        public float                timer;

        public List<AnimationFrame>         FramesList { get; }
        public AnimationFrame               Frame { get; private set; }
        
        public Animation(string p_animationName)
        {
            name = p_animationName;
            FramesList = new List<AnimationFrame>();
        }
        public void ResetAnimation()
        {
            timer = 0f;
            Frame = FramesList[0];
        }
        public void UpdateAnimation(float p_deltaTime)
        {
            if (Frame == null)
                Frame = FramesList[0];

            timer += p_deltaTime;
            float __tempTimer = timer;
            foreach (AnimationFrame __frame in FramesList)
            {
                __tempTimer -= __frame.duration;
                if (__tempTimer <= 0f)
                {
                    Frame = __frame;
                    return;
                }
            }
            timer = __tempTimer * -1f;
            Frame = FramesList[0];
        }
    }
}
