using System.Collections.Generic;
using UnityEngine;
using Valkirie.Core.Abilities;

namespace Valkirie.Core.Powers
{
    public class CharacterLoadout : MonoBehaviour
    {
        [SerializeField] private CharacterDefinition definition;
        [SerializeField] private MonoBehaviour motorBehaviour; // must implement IMotor (e.g. a MotorSwitcher)

        private IMotor _motor;
        private readonly List<PowerInstance> _powers = new();
        private PowerEffectContext _context;

        public IReadOnlyList<PowerInstance> Powers => _powers;

        private void Awake()
        {
            _motor = motorBehaviour as IMotor;
            if (_motor == null)
                Debug.LogError($"{name}: motorBehaviour does not implement IMotor.", this);

            _context = new PowerEffectContext(_motor, gameObject);

            foreach (var powerDef in definition.loadout)
                _powers.Add(new PowerInstance(powerDef));
        }

        private void Update()
        {
            foreach (var power in _powers)
                power.Tick(Time.deltaTime);
        }

        public bool TryActivate(string powerName)
        {
            var power = _powers.Find(p => p.Definition.displayName == powerName);
            return power != null && power.TryActivate(_context);
        }

        public void Deactivate(string powerName)
        {
            _powers.Find(p => p.Definition.displayName == powerName)?.Deactivate(_context);
        }
    }
}
