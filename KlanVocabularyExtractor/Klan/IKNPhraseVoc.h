// Copyright (c) 2004 by RRIAI. All rights reserved.
//===================================================================
// Usage notes: ������� ��.
//	����������������:
//		- ������� ����� �� ��������
//		- ��������
//		- ����������/��������������

//	����������� ���������������� (���������� ������������� ������) ���.:
//	- ��� ���� ��� ��: �������� ( norm ) + ������� ( rule ) = ���.�����

//===================================================================
// ������������ :   
//===================================================================
// ����������   :   IKNVoc, IKNPhrase
//===================================================================
// Oct 2005                  Created               L. Sidorova, RRIAI
//===================================================================
#ifndef IKNPhraseVocH
#define IKNPhraseVocH
//---------------------------------------------------------------------------
#include "IKNVoc.h"
#include "IKNPhrase.h"
//---------------------------------------------------------------------------
/** ������� ��. */
class IKNPhraseVoc :  public virtual IKNVoc {
public:
	/** ������ ���������� �������. ������� �������� � ���������� �������.
	 *  ��� ������� ���������� �������� � ���������� ���������, ��� �����������.
     *	@return 0 ���� ������ �������.
	 */
	virtual IKNPhrase* GetNextPhrase() = 0;

	/** ������ ���������� ������� (�������� �� ������ �������� � ���������� ���������).
	 *	@return 0 ���� ������ ������� (��� �� ����������).
	 */
	virtual IKNPhrase* GetSamePhrase() = 0;

	/** ������ ���������� ��, ����������� � ���� ������ �������� ������. */
	virtual IKNPhrase* GetNextInclude( IKNConcept* ) = 0;

	/** ����� ������� � ������� �� ��������� �����.
	 *	���� ����� �������� ���������, �� ������������ ��������� ������
	 *	�������� � ������ ���������� ������.
     *  ������ ������������ ������ �������� �� ��������� ������.
     *	��� ������������� ���� ������� �� ��������� �������� InitSame.
	 *	@return true - ���� ��� ��������������� ������ ��������� ���������.
	 */
	virtual bool FindConcept( char* iNorm, IKNPhrase** oTerm ) = 0;

	/** ����� �� � ��������� �����������.
     *	@param iRule - ��� �������, ������� �� �������������� �����.
     *	@param iMorh - ����� ����. �������� ��������� ������ - ������ ����, 
     *			������� ������ � �������� ���������� �������������� �����.
	 */
//	virtual IKNPhrase* FindPhrase( char* iNorm, char* iRule ) = 0; // old
	virtual IKNPhrase* FindPhrase( char* iNorm, const char* iRule, char* iMorh ) = 0;

	/** �������� ��. */
//	virtual void DeletePhrase( char* iNorm, char* iRule ) = 0;
	virtual void DeletePhrase( char* iNorm, const char* iRule, char* iMorh ) = 0;

	/** �������� ������� �� �������.
	 *	@note 	�������� �� ������� ������� ������� � ������� � 
	 *			������������ ���� ������� ������� �� ��������.
	 */
	virtual void DeletePhrase( IKNPhrase* iTerm ) = 0;

	/** ������� � �������� �� � �������.
	 * @note ��� �������� ������� � �������. ����� ���������� �������� ����� �� ��������� SetPart.
	 * @param iNorm - ���������� �����
	 * @param iRule - ��� �������, �� �������� ���������� �����
	 * @param iPartCount - ���-�� ��������� ������
	 */
	virtual IKNPhrase* CreatePhrase( char* iNorm, const char* iRule, unsigned iPartCount ) = 0;
};
//---------------------------------------------------------------------------
#endif
