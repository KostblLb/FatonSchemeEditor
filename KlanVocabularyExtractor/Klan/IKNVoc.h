// Copyright (c) 2004 by RRIAI. All rights reserved.
//===================================================================
// Usage notes: �������. �������� ������ ����� �������� �� �����������
//				� ������� ��������.
//	����������������:
//		- ������� ����� �� ��������
//		- ��������
//		- ����������/��������������

//	����������� ���������������� (���������� ������������� ������) ���.:
//	- ��� �����������: �������� ( norm )
//	- ��� ���� ��� ��: �������� ( norm ) + ������� ( rule ) = ���.�����
//	- ��� ����:		   �������� ( norm ) + ����. ����� ( morh ) +
//					   ������. ����� (�����)
//===================================================================
// Oct 2005                  Created               L. Sidorova, RRIAI
//===================================================================
#ifndef IKNVocH
#define IKNVocH
//---------------------------------------------------------------------------
//#include "IKNConcept.h"
//---------------------------------------------------------------------------
/** 
 * @interface IKNVoc
 * �������. ���������� ��������.
 */
class IKNVoc {
public:
    /** ���-�� �������� � �������. */
	virtual unsigned GetCount() const = 0;

    /** �������������� �������� ������� ��� ������������� ���������.
     *  ������ ������������ ������ �������� �� ��������� ������.
     */
	virtual void InitVoc() = 0;

	/** ������ ���������� �������. ������� �������� � ���������� �������.
	 *  ��� ������� ���������� �������� � ���������� ���������, ��� �����������.
     *	@return 0 ���� ������ �������.
	 */
//	virtual IKNConcept* GetNext() = 0;

	/** �������� ������� �� �������.
	 *	@note 	�������� �� ������� ������� ������� � ������� � 
	 *			������������ ���� ������� ������� �� ��������.
	 */
//	virtual void Delete( IKNConcept* iTerm ) = 0;

	/** ��������� ������� ���� �����/����������� ��������� �� �������. */
	virtual void ClearNews() = 0;

};
//---------------------------------------------------------------------------
#endif
