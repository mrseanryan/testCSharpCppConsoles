#pragma once
#include "fruit.h"

class COrange :
	public CFruit
{
public:
	COrange(void);
	virtual ~COrange(void);

	virtual EFruitType MyOverridablePublicMethod(); //hopefully this still over-rides the method in the grand-parent (CBase)

private:
	virtual EFruitType MyOverridablePrivateMethod();
};
