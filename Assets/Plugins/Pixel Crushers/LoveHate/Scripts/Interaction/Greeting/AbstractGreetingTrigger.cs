// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace PixelCrushers.LoveHate
{

    /// <summary>
    /// This abstract greeting trigger is the workhorse for GreetingTrigger (3D) and
    /// GreetingTrigger2D.
    /// </summary>
    public abstract class AbstractGreetingTrigger : AbstractTriggerInteractor
    {

        /// <summary>
        /// At least this many seconds must pass before greeting the same character.
        /// </summary>
        [Tooltip("This many seconds must pass before greeting the same character again.")]
        public float timeBetweenGreetings = 300f;

        /// <summary>
        /// The animations to play based on affinity.
        /// </summary>
        public RangeAnimation[] greetings = new RangeAnimation[3]
        {
            new RangeAnimation(string.Empty, -100, -25, RangeAnimation.AllTemperaments),
            new RangeAnimation(string.Empty, -25, 25, RangeAnimation.AllTemperaments),
            new RangeAnimation(string.Empty, 25, 100, RangeAnimation.AllTemperaments)
        };

        private Dictionary<FactionMember, float> lastGreeting = new Dictionary<FactionMember, float>();

        private FactionMember m_self = null;

        private Animator m_animator = null;

        protected virtual void Awake()
        {
            m_self = GetComponentInChildren<FactionMember>() ?? GetComponentInParent<FactionMember>();
            m_animator = GetComponentInChildren<Animator>() ?? GetComponentInParent<Animator>();
        }

        protected virtual void HandleOnTriggerEnter(GameObject other)
        {
            TryGreeting(GetFactionMember(other));
        }

        public virtual void TryGreeting(FactionMember other)
        {
            if (ShouldGreet(other))
            {
                Greet(other);
            }
        }

        protected virtual bool ShouldGreet(FactionMember other)
        {
            if (m_self == null || other == null || other == m_self) return false;
            var tooRecent = lastGreeting.ContainsKey(other) && (GameTime.time < (lastGreeting[other] + timeBetweenGreetings));
            return !tooRecent;
        }

        protected virtual void Greet(FactionMember other)
        {
            if (m_self == null || other == null || other == m_self || m_animator == null) return;
            lastGreeting[other] = GameTime.time;
            var affinity = m_self.GetAffinity(other);
            for (int g = 0; g < greetings.Length; g++)
            {
                var greeting = greetings[g];
                var isAppropriateGreeting = (greeting.min <= affinity && affinity <= greeting.max) &&
                    ((greeting.temperament & m_self.pad.GetTemperament()) != 0);
                if (isAppropriateGreeting)
                {
                    if (!string.IsNullOrEmpty(greeting.triggerParameter))
                    {
                        m_animator.SetTrigger(greeting.triggerParameter);
                    }
                    break;
                }
            }
            ExecuteEvents.Execute<IGreetEventHandler>(m_self.gameObject, null, (x, y) => x.OnGreet(other));
        }

    }

}
