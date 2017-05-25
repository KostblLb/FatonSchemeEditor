// Copyright (c) 2009 by IIS. All rights reserved.
//===================================================================
// Usage notes: ���������� ����������� ���������� ������� �� ����� ����.
//===================================================================
// ������������ :   IKNWord, IKNPhrase, KNConcept
//===================================================================
// ����������   :  
//===================================================================
// Oct 2009                  Created               L. Sidorova, IIS
//===================================================================
#ifndef IKNStatisticTH
#define IKNStatisticTH
//---------------------------------------------------------------------------
//#include "KNMacrosVoc.h"
#include "IKNTheme.h"
//---------------------------------------------------------------------------
/** ��������� ���� ������ ���������� ������� �� ����� ����. */
class IKNStatisticT
{
public:
	/** ����� ���������� ������ ����, � ������� ��������� ���������� �������. */
	virtual uint GetThemeId() const = 0;
	/** ����� ���������� ������ ����, � ������� ��������� ���������� �������. */
	virtual IKNTheme* GetTheme() const = 0;
	/** ����� ������������� ������� � ����. */
	virtual uint GetCount() const = 0;
	/** ����� ���-�� �������, � ������� ���������� ������. */
	virtual uint GetCountText() const = 0;
	/** ����� ���������� ������ ���� ������� � ����.
	 *	@note -1 ���� ������ �� �����������
	 */
	virtual int GetExpertNorm() const = 0;
	virtual void SetExpertNorm( int ) = 0;
	/** ����� ��� ������� � ����. ����� ����� ������� �������. */
//	virtual float GetDirectNorm() const = 0;
	/** ����� ������� ������� � ����. */	                               	
	virtual double GetRelativeNorm() const = 0;
	/** ����� ��� ������� � ����.
	 *	@note ��� ���� �������� ����������� �������� KNEngineStatistic::CalculateWeight
	 *	@param 	oStatus=0 ��������, ��� ��� �� ����������
	 *			oStatus=1 ��� � ��j����� ���������� (in progress)
	 *			oStatus=2 - ��� ��������
	 */
	virtual double GetWeight(byte_t *oStatus ) const = 0;
};

#endif
