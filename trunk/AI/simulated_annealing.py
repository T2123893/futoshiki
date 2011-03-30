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

File:    simulated_annealing.py
Date:    28/03/2011
Name:    Huang Huang (Rock)
ID:      T2123893
Version: V1.0
"""


import math
import random
from dataset import Dataset


class SimulatedAnnealing:
    def __init__(self):
        # randomly initialise a solution
        self.crt_solution = [random.choice([0,1]) for i in range (28)]
        self.crt_fitness = 0.
        # "current best" use to remember currently the highest one
        self.crt_best_solution = self.crt_solution[:]
        self.crt_best_fitness = 0.
        self.temperature = 1000000.0
        self.t_min = 0.1
        self.cool = 0.95
        # how many times the fitness no change
        self.no_change_time = 0
        self.old = 0.

    def calc_fitness(self, sln):
        ds = Dataset()
        weight = volume = fitness = 0.
        for i in range(len(sln)):
            if not sln[i]: continue # if item chosen
            weight += ds.items[i]['w']
            volume += ds.items[i]['v']
            fitness += ds.items[i]['p']
        if weight > ds.wmax or volume > ds.vmax:
            fitness = 0
        return fitness

    def solution_reset(self):
        """
        This feature is going to improve the simulated annealling search.
        If long time no change, restart current solution.
        Since fitness was no change more than 50 times,
        thus, reset 'crt_solution' In order to improve current solution
        """
        if self.no_change_time > 50:
            print ":::: Reset for improving solution seed"
            # print self.crt_solution
            self.crt_solution = [random.choice([0, 1]) for i in range(28)]
            # print self.crt_solution

    def recover_best(self):
        """
        This feature is going to improve the simulated annealling search.
        At the end of program, check and compare the best fitness so far with
        current fitness. If the current fitness is not the highest one then
        recover to the best fitness so far.
        """
        if self.crt_best_fitness > self.crt_fitness:
            self.crt_solution = self.crt_best_solution[:]
            self.crt_fitness = self.crt_best_fitness

    def remember_repeat_times(self, new_fitness):
        """
        This feature combine with solution reseting operation.
        Remember repeat times of new solution. Sometimes, the new solution will
        not be changed for long time. To remember this times, if time more than
        a threshold then reset solution in order to improve result.
        """
        if new_fitness == self.old:
            self.no_change_time += 1
            print "same fitness again, %d times" % (self.no_change_time)
        else:
            self.no_change_time = 0
            self.old = new_fitness
            print "fitness changed, set times to %d" % (self.no_change_time)

    def solve(self):
        while self.temperature > self.t_min:
            i = random.randint(0, len(self.crt_solution)-1)
            change = random.randint(0, 1)
            new_solution = self.crt_solution[:] # a copy from current solution
            new_solution[i] = abs(new_solution[i] - change) # change a value
            self.crt_fitness = self.calc_fitness(self.crt_solution)
            new_fitness = self.calc_fitness(new_solution)
            print "current:%f, new:%f, best:%f" % \
                  (self.crt_fitness, new_fitness, self.crt_best_fitness)
            self.remember_repeat_times(new_fitness)

            diff = new_fitness - self.crt_fitness
            p = pow(math.e, diff / self.temperature)
            if diff > 0:
                self.crt_solution = new_solution[:]
                if new_fitness > self.crt_best_fitness:
                    # remember the solution which has the highest fitness
                    self.crt_best_solution = new_solution[:]
                    self.crt_best_fitness = new_fitness
            elif diff < 0 and random.random() < p:
                self.crt_solution = new_solution[:]

            self.temperature *= self.cool
            self.solution_reset()

        self.recover_best()
        print self.crt_solution, self.crt_fitness


def main():
    sa = SimulatedAnnealing()
    print sa.crt_solution
    sa.solve()

if __name__ == "__main__":
    main()
