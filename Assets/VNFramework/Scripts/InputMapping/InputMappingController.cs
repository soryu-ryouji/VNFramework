using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VNFramework
{
    public class InputMappingController : MonoBehaviour, ICanGetModel
    {
        public AbstractKeyList abstractKeyList;
        public InputMappingList inputMappingList;
        public CompoundKeyRecorder compoundKeyRecorder;

        private AbstractKeyboardData keyboardData;

        private bool inited;

        private void Init()
        {
            if (inited)
            {
                return;
            }

            _inputMapper = VNutils.FindGameController().InputMapper;

            inited = true;

            RefreshData();
        }

        private InputMapper  _inputMapper;

        private InputMapper inputMapper
        {
            get
            {
                Init();
                return _inputMapper;
            }
        }

#if UNITY_EDITOR
        public IEnumerable<AbstractKey> mappableKeys => Enum.GetValues(typeof(AbstractKey)).Cast<AbstractKey>();
#else
        public IEnumerable<AbstractKey> mappableKeys => Enum.GetValues(typeof(AbstractKey)).Cast<AbstractKey>()
            .Where(ak => !inputMapper.keyIsEditor[ak]);
#endif

        private AbstractKey _currentAbstractKey;

        public AbstractKey CurrentAbstractKey
        {
            get => _currentAbstractKey;
            set
            {
                if (_currentAbstractKey == value)
                {
                    return;
                }

                _currentAbstractKey = value;
                abstractKeyList.RefreshSelection();
                inputMappingList.Refresh();
            }
        }

        public List<CompoundKey> CurrentCompoundKeys => keyboardData[CurrentAbstractKey];

        public void DeleteCompoundKey(int index)
        {
            CurrentCompoundKeys.RemoveAt(index);
            inputMappingList.Refresh();
        }

        public void AddCompoundKey()
        {
            CurrentCompoundKeys.Add(new CompoundKey());
            var lastEntry = inputMappingList.Refresh();
            StartModifyCompoundKey(lastEntry);
        }

        private void Start()
        {
            Init();
            _currentAbstractKey = mappableKeys.First();
            abstractKeyList.Refresh();
            inputMappingList.Refresh();
            compoundKeyRecorder.Init(this);
        }

        private void OnDisable()
        {
            Flush();
        }

        private void RefreshData()
        {
            keyboardData = inputMapper.keyboard.Data.GetCopy();
        }

        public void Flush()
        {
            inputMapper.keyboard.Data = keyboardData;
            inputMapper.Flush();
        }

        public void RestoreAll()
        {
            RefreshData();
            inputMappingList.Refresh();
        }

        public void RestoreCurrentKeyMapping()
        {
            keyboardData[CurrentAbstractKey] =
                inputMapper.keyboard.Data[CurrentAbstractKey].Select(key => new CompoundKey(key)).ToList();
            ResolveDuplicate();
            inputMappingList.Refresh();
        }

        public void ResetDefault()
        {
            keyboardData = inputMapper.GetDefaultKeyboardData();
            inputMappingList.Refresh();
        }

        public void ResetCurrentKeyMappingDefault()
        {
            keyboardData[CurrentAbstractKey] = inputMapper.GetDefaultCompoundKeys(CurrentAbstractKey);
            ResolveDuplicate();
            inputMappingList.Refresh();
        }

        public void StartModifyCompoundKey(InputMappingEntry entry)
        {
            compoundKeyRecorder.BeginRecording(entry);
        }

        // In all abstract keys other than currentAbstractKey that are in any same group as currentAbstractKey,
        // remove all compound keys that conflict with currentAbstractKey
        public void ResolveDuplicate()
        {
            foreach (var ak in keyboardData.Keys.ToList())
            {
                if (ak == CurrentAbstractKey)
                {
                    continue;
                }

                if ((inputMapper.keyGroups[ak] & inputMapper.keyGroups[CurrentAbstractKey]) == 0)
                {
                    continue;
                }

                keyboardData[ak] = keyboardData[ak].Where(key => !CurrentCompoundKeys.Contains(key)).ToList();
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}