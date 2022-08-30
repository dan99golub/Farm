using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Game;
using DTO;
using Lean.Gui;
using ServiceScript;
using Sirenix.Utilities;
using UnityEngine;

namespace Menu
{
    public class ButtonsControls : MonoBehaviour
    {
        public PlayerMark Player => Services<PlayerMark>.S.Get();

        public LeanButton UpBtn;
        public LeanButton RightBtn;
        public LeanButton DownBtn;
        public LeanButton LeftBtn;

        private Dictionary<AbsFieldMover.DirectionType, LeanButton> _buttons;
        private Dictionary<AbsFieldMover.DirectionType, LeanButton> Buttons => _buttons ??= new Dictionary<AbsFieldMover.DirectionType, LeanButton>()
        {
            {AbsFieldMover.DirectionType.Up, UpBtn},
            {AbsFieldMover.DirectionType.Right, RightBtn},
            {AbsFieldMover.DirectionType.Down, DownBtn},
            {AbsFieldMover.DirectionType.Left, LeftBtn},
        };

        private void Awake()
        {
            Player.FieldMover.NewDir += OnNewDir;
            Buttons.ForEach(x => x.Value.OnClick.AddListener(() => Player.FieldMover.Move(x.Key)));
        }
        
        public void MovePlayer(AbsFieldMover.DirectionType d) => Player.FieldMover.Move(d);

        private void OnNewDir(AbsFieldMover.DirectionType obj) => Buttons.ForEach(x => x.Value.interactable = ActiveByDir(obj, x.Key));

        private bool ActiveByDir(AbsFieldMover.DirectionType setDir, AbsFieldMover.DirectionType dirButton)
        {
            if (setDir == AbsFieldMover.DirectionType.None) return true;
            if (setDir == AbsFieldMover.DirectionType.Right || setDir == AbsFieldMover.DirectionType.Left)
            {
                if (dirButton == AbsFieldMover.DirectionType.Right || dirButton == AbsFieldMover.DirectionType.Left) return false;
                return true;
            }
            else
            {
                if (dirButton == AbsFieldMover.DirectionType.Up || dirButton == AbsFieldMover.DirectionType.Down) return false;
                return true;
            }
        }
    }
}