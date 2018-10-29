using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonster {
	// 자세한 내용
	void Recon();               // 정찰 모드
	void Attack();              // 공격 모드
	void Chase();				// 추적 모드
}

public interface IMonster_fixed{
	void Attack_fixed();		// 공격 모드
}