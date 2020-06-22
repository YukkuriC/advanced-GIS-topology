from sympy import *

for char in 'abcd':
    for pos in 'xy':
        name=char+pos
        globals()[name]=Symbol(name)

r1,r2=symbols('r1 r2')

eq1=(ax+(bx-ax)*r1)-(cx+(dx-cx)*r2)
eq2=(ay+(by-ay)*r1)-(cy+(dy-cy)*r2)

res=solve([eq1,eq2],[r1,r2])
print(res)