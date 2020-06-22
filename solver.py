from sympy import *

if 'BASIC':

    x, i, n = symbols('x i n')

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

    for char in 'mSy':
        globals()[char] = seq_helper()

    for grp in 'dx', 'dy', 'F', 'sh', 'su':
        globals()[grp] = seq_helper()

    seq_helper.bind_globals()

    _sinh = lambda e: (exp(e) - exp(-e)) / 2
    _sh = lambda e: sinh(S[e] * dx[e])
    _su = lambda e: sinh(S[e + 1] * dx[e])

    def str_(sym):  # 表达式字符串化
        return str(sym).replace('**', '^').replace(' ', '').replace(
            'exp', 'e^').replace('dx', '∆x').replace('dy', '∆y').replace(
                'S', 'σ')

    def run_(sym: Symbol, v):  # 代入函数
        return sym.subs({x: v})

    def rep_(sym: Symbol, t, f=i):
        """ 由expr_f替换至expr_t """
        if t == f:
            return sym
        rep_pool = {}
        for var in seq_helper.var_pool.values():
            if var.name == 'F':
                continue
            for offset in range(-1, 2):
                rep_pool[var[f + offset]] = var[t + offset]

        return sym.subs(rep_pool)

    def solve_(eq, i_seq):
        """ 获取eq内常数与所有m_i系数 """
        i_seq = tuple(i_seq)

        y_mapper = {y[i + 1]: dy[i] + y[i] for i in i_seq}

        # 获取常数
        const_mapper = {m[i]: 0 for i in i_seq}
        const_mapper.update(y_mapper)
        const = simplify(-eq.subs(const_mapper))  # eq=0 => 0=-eq
        eq_m = eq + const
        print(f"V_const={str_(const)}")

        # 逐个返回系数
        for curr in i_seq:
            mapper = {m[i]: int(i == curr) for i in i_seq}
            mapper.update(y_mapper)
            tmp = simplify(eq_m.subs(mapper))
            print(f"V_({str_(curr)})={str_(tmp)}")


F0 = m[i] * sinh(S[i] * (dx[i] - x)) / (S[i]**2 * sh[i])
F0 += m[i + 1] * sinh(S[i + 1] * x) / (S[i + 1]**2 * su[i])
F0 += (y[i] - m[i] / (S[i]**2)) * (dx[i] - x) / dx[i]
F0 += (y[i + 1] - m[i + 1] / (S[i + 1]**2)) * x / dx[i]

# 求导
F1 = simplify(diff(F0, x))
F2 = simplify(diff(F1, x))

# 测试端点函数值
if 'test f(x)' and 0:
    ftest = F0.subs({sh[i]: _sh(i), su[i]: _su(i)})
    assert run_(ftest, 0) == y[i] and run_(ftest, dx[i]) == y[i + 1]
    ftest = F2.subs({sh[i]: _sh(i), su[i]: _su(i)})
    assert simplify(run_(ftest, dx[i]) - run_(rep_(ftest, i + 1), 0)) == 0

print(f"F_i(x)={str_(F0)}")
print(f"F'_i(x)={str_(F1)}")
print(f"F''_i(x)={str_(F2)}")

# i=1~(n-1)
print('1.GENERAL'.center(30, '='))

eq = run_(rep_(F1, i - 1), dx[i - 1])
eq -= run_(F1, 0)
eq = simplify(eq)

solve_(eq, (i - 1 + o for o in range(3)))

# i=0, f'_0(0)=A
print('2.START'.center(30, '='))
eq = run_(rep_(F1, 0), 0) - Symbol('A')
solve_(eq, range(2))

# i=n-1, f'_(n-1)(dx_n-1)=B
print('3.END'.center(30, '='))
eq = run_(rep_(F1, n - 1), dx[n - 1]) - Symbol('B')
solve_(eq, (n - 1, n))
