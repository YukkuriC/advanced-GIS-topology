from sympy import *

if 'BASIC':

    x = Symbol('x')
    i = Symbol('i')

    js = [i + k for k in range(-1, 2)]

    class seq_helper:
        var_pool = {}

        def __init__(self):
            self.name = ''
            self.pool = {}

        def _name(self, i):
            return self.name + f'_({i})'

        def __getitem__(self, i):
            name = self._name(i)
            if i not in self.pool:
                self.pool[i] = Symbol(name)
            return self.pool[i]

        def __setitem__(self, i, v):
            self.pool[i] = v

        @classmethod
        def bind_globals(cls):
            for k, v in globals().items():
                if isinstance(v, cls):
                    v.name = k
                    cls.var_pool[k] = v

    for char in 'abcdσy':
        globals()[char] = seq_helper()

    for grp in 'dx', 'dy', 'F':
        globals()[grp] = seq_helper()

    seq_helper.bind_globals()

    def _sinh(e):
        return (exp(e) - exp(-e)) / 2

    def _cosh(e):
        return (exp(e) + exp(-e)) / 2

    def init_functions():
        F[0] = a[i] + b[i] * x + c[i] * _sinh(σ[i] * x) + d[i] * _cosh(
            σ[i] * x)
        F[1] = diff(F[0], x)
        F[2] = diff(F[1], x)

    def str_(sym):
        return str(sym).replace('**', '^').replace(' ', '').replace(
            'exp', 'e^').replace('*', ' ').replace('dx', '∆x')

    def run_(sym: Symbol, v):
        return sym.subs({x: v})

    def rep_(sym: Symbol, t, f=i):
        if t == f:
            return sym
        rep_pool = {}
        for var in seq_helper.var_pool.values():
            rep_pool[var[f]] = var[t]

        return sym.subs(rep_pool)

    def solve_(eq, sym_name, ind=i):
        target_sym = globals()[sym_name]
        target = target_sym[ind]
        res = simplify(solve(eq, target)[0])
        res_str = str_(res)
        print(f'{sym_name}_({ind})={res_str}')
        for j in js:
            target_sym[j] = rep_(res, j)
        init_functions()


init_functions()

# 1. solve A (F_i(0)=y_i)
eq = run_(F[0], 0) - y[i]
solve_(eq, 'a')

# 2. solve C (F''_i(dx_i)=F''_i+1(0))
eq = run_(F[2], dx[i])
eq -= run_(rep_(F[2], i + 1), 0)
solve_(eq, 'c')

# 3. solve B (F_i(dx_i)=F_i+1(0)=y_i+1)
eq = run_(F[0], dx[i])
eq -= y[i + 1]
solve_(eq, 'b')

# 4. final form (F'_i-1(dx_i)=F'_i(0))
X, Y, Z = symbols('X Y Z')
eq = run_(rep_(F[1], i - 1), dx[i - 1])
eq -= run_(F[1], 0)
eq = eq.subs({d[i - 1]: X, d[i]: Y, d[i + 1]: Z})

print('=' * 30)
print(str_(eq))

base = simplify(-eq.subs({X: 0, Y: 0, Z: 0}))
eq = eq + base

print('-' * 30)
print(str_(eq.subs({X: 0, Y: 0, Z: 0})))
print('=' * 30)

print("V_(i-1)=" + str_(simplify(eq.subs({X: 1, Y: 0, Z: 0}))))
print("V_i=" + str_(simplify(eq.subs({X: 0, Y: 1, Z: 0}))))
print("V_(i+1)=" + str_(simplify(eq.subs({X: 0, Y: 0, Z: 1}))))
print("V_0=" + str_(base))
