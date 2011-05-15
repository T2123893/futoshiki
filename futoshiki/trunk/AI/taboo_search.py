#! /usr/bin/python
# *-* coding: utf-8 *-*

"""
$Id$

COMP40511 - Applied Artificial Intelligence

This file is the result of my own work. Any contributions to the work by
third parties, other than tutors, are stated clearly below this declaration.
Should this statement prove to be untrue I recognise the right and duty of
the Board of Examiners to take appropriate action in line with the university's
regulations on assessment.

File:    taboo_search.py
Date:    28/03/2011
Name:    Huang Huang (Rock)
ID:      T2123893
Version: V1.0
"""


import copy
import random
from dataset import Dataset


class Solution:
    """
    This class represents a solution.
    """
    def __init__(self, l=28):
        self.l = l # the length of solution
        init_v = [random.randint(0,1) for i in range (l)]
        self.set_v(init_v)

    def set_v(self, v):
        """
        solution value setting. When setting solution value, the fitness will
        bechanged as well.
        """
        self.v = v
        self.set_f(v)

    def set_f(self, sv):
        self.f = Solution.calc_fitness(sv)

    @staticmethod
    def calc_fitness(sv):
        """
        A fitness calculating operation. It is designed as static method. Other
        places can use this operation to calculate a temp fitness without
        create a instance of Solution.
        """
        ds = Dataset()
        weight = volume = fitness = 0.
        for i in range(len(sv)):
            if not sv[i]: continue # if item chosen
            weight += ds.items[i]['w']
            volume += ds.items[i]['v']
            fitness += ds.items[i]['p']
        if weight > ds.wmax or volume > ds.vmax:
            fitness = 0
        return fitness

class TabooSearch:
    """
    This algorithm is based on Taboo Search algorithm. It has changed a little,
    which uses two taboo list. One is for generating neighbour solution, to
    tell program which solution is useless. Another one is using to remember
    actual solution searching.
    """
    def __init__(self, iterations=100):
        self.crt_sln = Solution()
        self.tabu = [] # Initialise taboo list
        self.tabu_size = self.crt_sln.l
        self.crt_best_sln = copy.deepcopy(self.crt_sln) # set best so far
        self.iter = iterations

    def solve(self):
        while self.iter >= 0:
            neighbors = [] # collection of neighbor solutions
            itabu = []  # taboo some position which is already accessed
            for i in range (self.crt_sln.l):
                i = random.randint(0, self.crt_sln.l - 1)
                change = random.randint(0, 1)
                if (i, change) in itabu: continue
                temp_sln = Solution()
                temp_v = self.crt_sln.v[:]
                temp_v[i] = abs(temp_v[i] - change)
                temp_sln.set_v(temp_v[:])
                if temp_sln.f > self.crt_sln.f:
                    neighbors.append(temp_sln)
                else:
                    itabu.append((i, change))
            if 0 == len(neighbors):
                # sometimes better initialise, better solution
                print "neighbors are empty, resart initial solution----------"
                new_v = [random.randint(0,1) for i in range (self.crt_sln.l)]
                self.crt_sln.set_v(new_v)

            # candidates searching
            for sln in neighbors:
                exist = sln.v in self.tabu
                if not exist or (exist and sln.f > self.crt_best_sln.f):
                    self.crt_sln.set_v(sln.v[:])
                # update taboo list
                if not exist and len(self.tabu) <= self.tabu_size:
                    if len(self.tabu) > 0:
                        f = [Solution.calc_fitness(sv) for sv in self.tabu]
                        m = min(f)
                        self.tabu.pop(f.index(m))
                    self.tabu.append(sln.v)

            # set best so far
            if self.crt_sln.f > self.crt_best_sln.f:
                self.crt_best_sln.set_v(self.crt_sln.v[:])

            print "%d: current fitness:%f" % (self.iter, self.crt_sln.f)
            self.iter -= 1

def main():
    ts = TabooSearch()
    ts.solve()
    print "Best so far: %f" % (ts.crt_best_sln.f)

if __name__ == "__main__":
    main()
