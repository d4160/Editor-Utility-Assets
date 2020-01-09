namespace Hierponent { // v1.0.2
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	public class Hierponent {



		#region --- VAR ---


		private const int MAX_ICON_NUM = 7;

		private static List<System.Type> HideTypes = new List<System.Type>() {
			typeof(Transform), typeof(ParticleSystemRenderer)
		};
        private static Transform OffsetObject = null;
		private static int Offset = 0;


		#endregion



		#region --- MSG ---



		[InitializeOnLoadMethod]
		public static void Init () {
			EditorApplication.hierarchyWindowItemOnGUI += HieGUI;
		}





		public static void HieGUI (int instanceID, Rect rect) {


			// Check
			Object tempObj = EditorUtility.InstanceIDToObject(instanceID);
			if (!tempObj) {
				return;
			}


			// fix rect
			rect.width += rect.x;
			rect.x = 0;


			// Logic
			GameObject obj = tempObj as GameObject;
			List<Component> coms = new List<Component>(obj.GetComponents<Component>());
			for (int i = 0; i < coms.Count; i++) {
				if (!coms[i]) {
					continue;
				}
				if (TypeCheck(coms[i].GetType())) {
					coms.RemoveAt(i);
					i--;
				}
			}

			int iconSize = 16;
			int y = 1;
			int offset = obj.transform == OffsetObject ? Offset : 0;

			// Main
			for (int i = 0; i + offset < coms.Count && i < MAX_ICON_NUM; i++) {

				Component com = coms[i + offset];

				// Logic
				Texture2D texture = AssetPreview.GetMiniThumbnail(com);
                Rect texRect = new Rect(
                    rect.width - (iconSize + 1) * (i + 2),
                    rect.y + y,
                    iconSize,
                    iconSize);

                if (texture) {
                    
                    var beh = com as Behaviour;

                    var content = new GUIContent("", com.GetType().ToString());

                    if (beh)
                    {
                        DrawComponentButton(beh, texRect, content, texture);
                    }
                    else
                    {
                        var col = com as Collider;
                        if (col)
                        {
                            DrawComponentButton(col, texRect, content, texture);
                        }
                        else
                        {
                            var ren = com as Renderer;

                            if (ren)
                                DrawComponentButton(ren, texRect, content, texture);
                            else
                            {
                                GUI.Label(texRect, content);
                                GUI.DrawTexture(texRect, texture);
                            }
                        }
                    }
				}
			}


			// More Button
			if (coms.Count == MAX_ICON_NUM + 1) {
				Texture2D texture = AssetPreview.GetMiniThumbnail(coms[coms.Count - 1]);
				if (texture) {
					GUI.DrawTexture(new Rect(
						rect.width - (iconSize + 1) * (coms.Count - 1 + 2),
						rect.y + y,
						iconSize,
						iconSize
					), texture);
				}
			} else if (coms.Count > MAX_ICON_NUM) {

				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.fontSize = 9;
				style.alignment = TextAnchor.MiddleCenter;

				if (GUI.Button(new Rect(
					rect.width - (iconSize + 2) * (MAX_ICON_NUM + 2),
					rect.y + y,
					22,
					iconSize
				), "•••", style)) {
					if (OffsetObject != obj.transform) {
						OffsetObject = obj.transform;
						Offset = 0;
					}
					Offset += MAX_ICON_NUM;
					if (Offset >= coms.Count) {
						Offset = 0;
					}
				}

			}
		}


        private static void DrawComponentButton(Behaviour beh, Rect rect, GUIContent content, Texture tex)
        {
            var prevColor = GUI.color;
            var alpha = beh.enabled ? 1f : 0.25f;
            
            rect.y += 5;
            rect.width -= 5;
            rect.height -= 5;

            prevColor.a = 0;
            GUI.color = prevColor;
            if (GUI.Button(rect, content))
            {
                Undo.RecordObject(beh, "Component.enabled");
                beh.enabled = !beh.enabled;
            }

            rect.y -= 5;
            rect.width += 5;
            rect.height += 5;

            prevColor.a = alpha;
            GUI.color = prevColor;
            GUI.DrawTexture(rect, tex);

            GUI.color = Color.red;

            rect.x += rect.width - 5;
            rect.width = 5;
            rect.height = 5;

            if (GUI.Button(rect, new GUIContent("", "Destroy component?")))
            {
                Undo.DestroyObjectImmediate(beh);
            }

            GUI.color = prevColor;
        }

        private static void DrawComponentButton(Collider beh, Rect rect, GUIContent content, Texture tex)
        {
            var prevColor = GUI.color;
            var alpha = beh.enabled ? 1f : 0.25f;

            rect.y += 5;
            rect.width -= 5;
            rect.height -= 5;

            prevColor.a = 0;
            GUI.color = prevColor;
            if (GUI.Button(rect, content))
            {
                Undo.RecordObject(beh, "Component.enabled");
                beh.enabled = !beh.enabled;
            }

            rect.y -= 5;
            rect.width += 5;
            rect.height += 5;

            prevColor.a = alpha;
            GUI.color = prevColor;
            GUI.DrawTexture(rect, tex);

            GUI.color = Color.red;

            rect.x += rect.width - 5;
            rect.width = 5;
            rect.height = 5;

            if (GUI.Button(rect, new GUIContent("", "Destroy component?")))
            {
                Undo.DestroyObjectImmediate(beh);
            }

            GUI.color = prevColor;
        }

        private static void DrawComponentButton(Renderer beh, Rect rect, GUIContent content, Texture tex)
        {
            var prevColor = GUI.color;
            var alpha = beh.enabled ? 1f : 0.25f;

            rect.y += 5;
            rect.width -= 5;
            rect.height -= 5;

            prevColor.a = 0;
            GUI.color = prevColor;
            if (GUI.Button(rect, content))
            {
                Undo.RecordObject(beh, "Component.enabled");
                beh.enabled = !beh.enabled;
            }

            rect.y -= 5;
            rect.width += 5;
            rect.height += 5;

            prevColor.a = alpha;
            GUI.color = prevColor;
            GUI.DrawTexture(rect, tex);

            GUI.color = Color.red;

            rect.x += rect.width - 5;
            rect.width = 5;
            rect.height = 5;

            if (GUI.Button(rect, new GUIContent("", "Destroy component?")))
            {
                Undo.DestroyObjectImmediate(beh);
            }

            GUI.color = prevColor;
        }
        #endregion



        #region --- LGC ---


        private static bool TypeCheck (System.Type type) {
			for (int i = 0; i < HideTypes.Count; i++) {
				if (type == HideTypes[i] || type.IsSubclassOf(HideTypes[i])) {
					return true;
				}
			}
			return false;
		}


		#endregion


	}
}