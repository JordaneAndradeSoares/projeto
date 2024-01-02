using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations;
using System.Data;

namespace modoBatalha
{
    public class GerenciadorAttVida : MonoBehaviour
    {
        public RawImage vidaPerdida;
        public LookAtConstraint LookAtConstraint;
<<<<<<< Updated upstream
        public TextMeshProUGUI nome;

=======

        public TextMeshProUGUI txt;
>>>>>>> Stashed changes
        private void Start()
        {
            ConstraintSource a = LookAtConstraint.GetSource(0);
            a.sourceTransform = Camera.main.transform;
            LookAtConstraint.SetSource(0, a);

            
        }

    }
}
