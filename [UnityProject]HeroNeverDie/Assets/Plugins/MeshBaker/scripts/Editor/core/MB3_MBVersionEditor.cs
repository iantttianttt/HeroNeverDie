/**
 *	DLLs cannot interpret preprocessor directives, so this class acts as a "bridge"
 */
using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DigitalOpus.MB.Core{

	public interface MBVersionEditorInterface{
		string GetPlatformString(); 
		void RegisterUndo(UnityEngine.Object o, string s);
		void SetInspectorLabelWidth(float width);
        void UpdateIfDirtyOrScript(SerializedObject so);
    }
	
	public class MBVersionEditor
	{
		private static MBVersionEditorInterface _MBVersion;

		private static MBVersionEditorInterface _CreateMBVersionConcrete(){
			Type vit = null;
			#if EVAL_VERSION
			vit = Type.GetType("DigitalOpus.MB.Core.MBVersionEditorConcrete,Assembly-CSharp-Editor");
			#else
			vit = typeof(MBVersionEditorConcrete);
			#endif
			return (MBVersionEditorInterface) Activator.CreateInstance(vit);
		}

		public static string GetPlatformString(){
			if (_MBVersion == null) _MBVersion = _CreateMBVersionConcrete();
			return _MBVersion.GetPlatformString();
		}

		public static void RegisterUndo(UnityEngine.Object o, string s){
			if (_MBVersion == null) _MBVersion = _CreateMBVersionConcrete();
			_MBVersion.RegisterUndo(o,s);
		}

		public static void SetInspectorLabelWidth(float width){
			if (_MBVersion == null) _MBVersion = _CreateMBVersionConcrete();
			_MBVersion.SetInspectorLabelWidth(width);
		}

        public static void UpdateIfDirtyOrScript(SerializedObject so)
        {
            if (_MBVersion == null) _MBVersion = _CreateMBVersionConcrete();
            _MBVersion.UpdateIfDirtyOrScript(so);
        }
    }
}