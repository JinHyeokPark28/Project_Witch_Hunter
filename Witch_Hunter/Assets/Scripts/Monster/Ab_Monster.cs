using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ab_Monster : MonoBehaviour {
	// 자세한 내용
	public abstract void Recon();               // 정찰 모드
	public abstract void Attack();              // 공격 모드
	public abstract void Chase();               // 추적 모드
	public abstract void Hp();					// 체력
	public abstract void Damage();				// 데미지
}

public abstract class Ab_Monster_Fixed : MonoBehaviour{
	public abstract void Attack_fixed();        // 공격 모드
	public abstract void Hp();					// 체력
	public abstract void Damage();				// 데미지
}