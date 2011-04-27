#pragma once

enum EFruitType { BASE, ORANGE };

class CFruitBase
{
public:
	CFruitBase(void);
	virtual ~CFruitBase(void);

	EFruitType CallPrivateMethod();

	virtual EFruitType MyOverridablePublicMethod();

private:
	virtual EFruitType MyOverridablePrivateMethod();
};
