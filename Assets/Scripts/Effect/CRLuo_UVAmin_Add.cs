using UnityEngine;
using System.Collections;

public class CRLuo_UVAmin_Add : MonoBehaviour
{
	public string _ = "-=<λ�Ʋ�����UV����>=-";
	public string __ = "���򿪹�";
	public bool Use = false;
	public string ___ = "���������";
	public Material myMaterial;

	public string ____ = "UVλ��ֵ";
	public float UAdd;
	public float VAdd;

	float UNow;

	float VNow;
	
	void Start(){
		//���ó�ʼֵ
		UNow = 0;
		VNow = 0;
		//���ò������UVλ��
		if (myMaterial != null)
		{
			myMaterial.SetTextureOffset("_MainTex", new Vector2(UNow, VNow));
		}
		else
		{

			this.gameObject.renderer.material.SetTextureOffset("_MainTex", new Vector2(UNow, VNow));
		}
	}

	void Update()
	{
		if (Use)
		{
			//�����ۼ�ֵ
			UNow += UAdd * Time.deltaTime;
			VNow += VAdd * Time.deltaTime;

			UNow = UNow % 1;
			VNow = VNow % 1;
			//���ò������UVλ��
			if (myMaterial != null)
			{
				myMaterial.SetTextureOffset("_MainTex", new Vector2(UNow, VNow));
			}
			else
			{

				this.gameObject.renderer.material.SetTextureOffset("_MainTex", new Vector2(UNow, VNow));
			}
		}
	}
}
