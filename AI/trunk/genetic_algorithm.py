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

File:    genetic_algorithm.py
Date:    28/03/2011
Name:    Huang Huang (Rock)
ID:      T2123893
Version: V1.0
"""


import copy
import random
from dataset import Dataset

class Individual:
    """ This class is describing a individual container. """

    def __init__(self):
        self.fitness = 0
        self.fit_ratio = 0 # a ratio of fitness than current highest fitness
        self.chrom = [random.choice([0,1]) for i in range (28)]


class SimpleGeneticAlgorithm:
    """ This class is to implement a Simple Genetic Algorithm """

    def __init__(self, popsize, maxgen,
                 pcrossover, pmutation):
        self.popsize = popsize # population size
        self.maxgen = maxgen # maximum iterate generation
        self.current= [] # current genepool
        for i in range(popsize): # initialise genepool
            self.current.append(Individual())
        self.pcrossover = pcrossover # probability of crossover
        self.pmutation = pmutation # probability of mutation
        self.ds = Dataset()

    def calc_fitness(self, gnm):
        """ calculating fitness for individual """
        weight = volume = fitness = 0.
        for n in range(len(gnm)):
            if 0 == gnm[n]: continue # if item chosen
            weight += self.ds.items[n]['w']
            volume += self.ds.items[n]['v']
            fitness += self.ds.items[n]['p']
        if weight > self.ds.wmax or volume > self.ds.vmax:
            over_weight = weight / self.ds.wmax
            over_volume = volume / self.ds.vmax
            fitness = fitness / (over_weight + over_volume)
        return fitness

    def evaluate(self):
        """ evaluating current population """
        for i in self.current: # calculate fitness
            i.fitness = self.calc_fitness(i.chrom)
        m = max([i.fitness for i in self.current]) # the highest fitness
        # calculate ratio that fitness than the current highest fitness
        for i in self.current:
            i.fit_ratio = i.fitness / float(m)

    def select(self):
        """ select individuals based on Roulette wheel selection """
        next = [] # next generation
        # high fitness with higher survival rate
        for i in self.current:
            x = abs(random.gauss(0, 1./3))
            if x > 1: x = 1
            if x <= i.fit_ratio:
                # keep winning individual in 'roulette'
                next.append(i)
        self.current = next[:]

    def cross_pair(self, d, m):
        locus = random.randint(0, 27) # generate a cross point randomly
        gnm1 = d.chrom[:locus] + m.chrom[locus:]
        gnm2 = m.chrom[:locus] + d.chrom[locus:]
        # to replace when fitness of offspring higher than parent
        if self.calc_fitness(gnm1) > d.fitness:
            d.chrom = gnm1
        elif self.calc_fitness(gnm2) > d.fitness:
            d.chrom = gnm2
        elif self.calc_fitness(gnm1) > m.fitness:
            m.chrom = gnm1
        elif self.calc_fitness(gnm2) > m.fitness:
            m.chrom = gnm2

    def crossover(self):
        for i in self.current: # choose individual randomly
            j = int(random.uniform(0, len(self.current)))
            x = random.random()
            if x <= self.pcrossover: # do crossover under probability crossover
                self.cross_pair(i, self.current[j])

    def single_point_mutation(self, i):
        temp_chrom = copy.deepcopy(i.chrom)
        p = random.randint(0, 27)
        temp_chrom[p] = abs(temp_chrom[p]-1)
        if self.calc_fitness(temp_chrom) > i.fitness:
            # keep it after mutating if fitness higher than original
            i.chrom = temp_chrom

    def mutation(self):
        for i in self.current: # choose individual randomly
            x = random.random()
            if x <= self.pmutation: # do mutation under probability of mutation
                self.single_point_mutation(i)

    def solve(self):
        for i in range(self.maxgen):
            self.evaluate() # fitness evaluating
            self.select()
            f = [j.fitness for j in self.current]
            m = max(f)
            #输出次数，最高适应度，最高适应度个体
            print "%d: highest:%6.4f individual:%s" % \
                  (i, m, self.current[f.index(m)].chrom)
            self.crossover()
            self.mutation()
            if len(self.current) == 1:
                print 'break at', i
                break


def main():
    g = SimpleGeneticAlgorithm(popsize=100, maxgen=50,
            pcrossover=.90, pmutation=.01)
    g.solve()

if __name__ == "__main__":
    main()
