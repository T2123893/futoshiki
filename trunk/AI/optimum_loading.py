#! /usr/bin/python
# *-* coding: utf-8 *-*

"""
$Id: genetic_algorithm.py 17 2011-03-30 22:55:34Z adriftrock@gmail.com $

COMP40511 - Applied Artificial Intelligence

This file is the result of my own work. Any contributions to the work by
third parties, other than tutors, are stated clearly below this declaration.
Should this statement prove to be untrue I recognise the right and duty of
the Board of Examiners to take appropriate action in line with the university's
regulations on assessment.

File:    optimum_loading.py
Date:    28/03/2011
Name:    Huang Huang (Rock)
ID:      T2123893
Version: V1.0
"""

from Tkinter import *
from matplotlib import pyplot
from xlrd.biffh import XLRDError

import tkFileDialog
import tkMessageBox
import copy
import math
import random
import xlrd
import sys
import Pmw

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

        self.xdata = []                   # Test
        self.ydata = []                   # Test
        self.iter1 = 0                    # Test
        self.xdata.append(self.iter1)     # Test
        self.ydata.append(self.crt_sln.f) # Test

    def solve(self):
        while self.iter >= 0:
            neighbors = [] # collection of neighbor solutions
            itabu = []  # taboo some position which is already accessed
            for i in range (self.crt_sln.l):
                ind = random.randint(0, self.crt_sln.l - 1)
                change = random.randint(0, 1)
                if (ind, change) in itabu: continue
                temp_sln = Solution()
                temp_v = self.crt_sln.v[:]
                temp_v[i] = abs(temp_v[i] - change)
                temp_sln.set_v(temp_v[:])
                if temp_sln.f > self.crt_sln.f:
                    neighbors.append(temp_sln)
                else:
                    itabu.append((ind, change))
            if 0 == len(neighbors):
                # sometimes better initialise, better solution
                print "--neighbors are empty, resart initial solution----------"
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

            print "%d: current fitness:%.2f" % (self.iter, self.crt_sln.f)
            self.iter -= 1
            self.ydata.append(self.crt_sln.f)   # Test
            self.iter1 += 1                     # Test
            self.xdata.append(self.iter1)       # Test
        print "Best fitness so far: %.2f" % self.crt_best_sln.f
        return self.xdata, self.ydata           # Test

################################################################################

class SimulatedAnnealing:
    def __init__(self):
        self.crt_sln = Solution() # initialise a solution
        self.best_so_far = copy.deepcopy(self.crt_sln) # remember currently best
        self.temperature = 100000.0 # the initial temperature
        self.t_min = 0.1    # the terminational temperature
        self.cool = 0.95    # cooling ratio
        self.no_change = 0 # counter how many times the fitness no change
        self.lastfit = 0.   # the fitness of last time

        self.xdata = []      # Test
        self.ydata = []      # Test
        self.iter = 0        # Test

    def solution_reset(self):
        """
        This feature is going to improve the simulated annealling search.
        If long time no change, restart current solution.
        Since fitness was no change more than 50 times,
        thus, reset 'crt_sln' In order to improve current solution
        """
        if self.no_change > 50:
            print ":::: Reset for improving solution seed"
            self.crt_sln = Solution()

    def recover_best(self):
        """
        This feature is going to improve the simulated annealling search.
        At the end of program, check and compare the best fitness so far with
        current fitness. If the current fitness is not the highest one then
        recover to the best fitness so far.
        """
        if self.best_so_far.f > self.crt_sln.f:
            self.crt_sln.set_v(self.best_so_far.v[:])
        print "Best so far: %.2f" % self.best_so_far.f

    def remember_repeat_times(self, new_fitness):
        """
        This feature combine with solution reseting operation.
        Remember repeat times of new solution. Sometimes, the new solution will
        not be changed for long time. To remember this times, if time more than
        a threshold then reset solution in order to improve result.
        """
        if new_fitness == self.lastfit:
            self.no_change += 1
            print "same fitness again, %d times" % (self.no_change)
        else:
            self.no_change = 0
            self.lastfit = new_fitness
            print "fitness has changed, set counter to %d" % (self.no_change)

    def solve(self):
        while self.temperature > self.t_min:
            i = random.randint(0, self.crt_sln.l - 1)
            change = random.randint(0, 1)
            temp_sln = Solution()
            temp_v = self.crt_sln.v[:] # a copy from current solution
            temp_v[i] = abs(temp_v[i] - change) # change a value
            temp_sln.set_v(temp_v[:])
            self.ydata.append(self.crt_sln.f) # Test
            print "%d:temperature:%.1f, current fitness:%.2f, temp:%.2f" % \
                  (self.iter, self.temperature, self.crt_sln.f, temp_sln.f)
            self.remember_repeat_times(temp_sln.f)

            diff = temp_sln.f - self.crt_sln.f
            p = pow(math.e, diff / self.temperature)
            if diff > 0:
                self.crt_sln.set_v(temp_v)
                if temp_sln.f > self.best_so_far.f:
                    # remember the solution which has the highest fitness
                    self.best_so_far.set_v(temp_v)
                    print "Best fitness so far:%.2f" % (self.best_so_far.f)
            elif diff < 0 and random.random() < p:
                self.crt_sln.set_v(temp_v)

            self.temperature *= self.cool

            self.iter += 1                # Test
            self.xdata.append(self.iter)  # Test
            self.solution_reset()

        self.recover_best()
        return self.xdata, self.ydata

################################################################################

class GeneticAlgorithm:
    """ This class is to implement a Simple Genetic Algorithm """

    def __init__(self, popsize=100, maxgen=50, pcrossover=0.90, pmutation=.01):
        self.popsize = popsize # population size
        self.maxgen = maxgen # maximum iterate generation
        self.current= [] # current genepool
        for i in range(popsize): # initialise genepool
            self.current.append(Solution())
        self.pcrossover = pcrossover # probability of crossover
        self.pmutation = pmutation # probability of mutation
        self.ds = Dataset()
        self.xdata = []      # Test
        self.ydata = []      # Test
        self.iter = 0        # Test

    def evaluate(self):
        """ evaluating current population """
        m = max([i.f for i in self.current]) # the highest fitness
        # calculate ratio that fitness than the current highest fitness
        for i in self.current:
            i.fit_ratio = i.f / float(m)

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
        gnm1 = d.v[:locus] + m.v[locus:]
        gnm2 = m.v[:locus] + d.v[locus:]
        # to replace when fitness of offspring higher than parent
        if Solution.calc_fitness(gnm1) > d.f:
            d.set_v(gnm1[:])
        elif Solution.calc_fitness(gnm2) > d.f:
            d.set_v(gnm2[:])
        elif Solution.calc_fitness(gnm1) > m.f:
            m.set_v(gnm1[:])
        elif Solution.calc_fitness(gnm2) > m.f:
            m.set_v(gnm2[:])

    def crossover(self):
        for i in self.current: # choose individual randomly
            j = int(random.uniform(0, len(self.current)))
            x = random.random()
            if x <= self.pcrossover: # do crossover under probability crossover
                self.cross_pair(i, self.current[j])

    def single_point_mutation(self, i):
        temp_v = i.v[:]
        p = random.randint(0, 27)
        temp_v[p] = abs(temp_v[p]-1)
        if Solution.calc_fitness(temp_v) > i.f:
            # keep it after mutating if fitness higher than original
            i.set_v(temp_v[:])

    def mutation(self):
        for i in self.current: # choose individual randomly
            x = random.random()
            if x <= self.pmutation: # do mutation under probability of mutation
                self.single_point_mutation(i)

    def solve(self):
        for i in range(self.maxgen):
            self.xdata.append(i) # Test
            self.evaluate() # fitness evaluating
            self.select()
            fits = [j.f for j in self.current]
            m = max(fits)
            self.ydata.append(m) # Test

            print "%d: highest:%6.4f individual:%s" % \
                  (i, m, self.current[fits.index(m)].v)
            self.crossover()
            self.mutation()
            if len(self.current) == 1:
                print 'break at', i
                break
        return self.xdata, self.ydata

################################################################################

class Solution:
    """
    This class represents a solution.
    """
    ga = False # to indicate whether the solution is for GA or not

    def __init__(self, l=28):
        self.l = l # the length of solution
        init_v = [random.randint(0,1) for i in range (l)]
        self.fit_ratio = 0.
        self.set_v(init_v)

    def set_v(self, v):
        """
        solution value setting. When setting solution value, the fitness will
        be changed as well.
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
        weight = volume = fitness = 0.
        for i in range(len(sv)):
            if not sv[i]: continue # if item chosen
            weight += Dataset.items[i]['w']
            volume += Dataset.items[i]['v']
            fitness += Dataset.items[i]['p']
        if weight > Dataset.wmax or volume > Dataset.vmax:
            if Solution.ga:
                over_weight = weight / Dataset.wmax
                over_volume = volume / Dataset.vmax
                fitness = fitness / (over_weight + over_volume)
            else:
                fitness = 0
        return fitness

################################################################################

class Dataset:
    """ A class that to load data from specified spreadsheet. """
    ds = None
    name = ""
    wmax = 0
    vmax = 0
    fmax = 0
    items = {}

    @staticmethod
    def loaddataset(filename):
        try:
            Dataset.ds = xlrd.open_workbook(filename)
            sheet = Dataset.ds.sheet_by_index(0)
            Dataset.name = sheet.name
            Dataset.wmax = sheet.cell_value(rowx=2, colx=1)
            Dataset.vmax = sheet.cell_value(rowx=3, colx=1)
            Dataset.fmax = 0.
            for r in range(7, 35):
                Dataset.items[r - 7] = {'w': sheet.cell(r, 1).value,
                                        'v': sheet.cell(r, 2).value,
                                        'p': sheet.cell(r, 3).value}
                Dataset.fmax += sheet.cell(r, 3).value # the total fitness
        except XLRDError, x:
            print XLRDError, ":", x
            tkMessageBox.showinfo("Error", x)

################################################################################

class AppWin(Frame):
    """Display console information"""

    def __init__( self ):
        Frame.__init__( self )
        Pmw.initialise()
        self.pack( expand = YES, fill = BOTH )
        self.master.title( "Optimum Loading of a Standard Container" )

        #create scrolled text box
        self.cmd = Pmw.ScrolledText(self, text_width = 120, text_height = 20,
                                    text_wrap = WORD, hscrollmode = "static",
                                    vscrollmode = "static")
        self.cmd.pack( side = BOTTOM, expand = YES, fill = BOTH, padx = 5,
                       pady = 5 )
        self.cmd.component("text").configure(background = "black",
                                             foreground = "green")

        menubar = Menu(self.master) # top menu (horizontal)
        # sub menu (vertical)
        run_menu = Menu(menubar, tearoff=0)
        opt_menu = Menu(menubar, tearoff=0)
        # items of sub menu
        run_menu.add_command(label="Simulated Annealing", command=self.sa)
        run_menu.add_command(label="Taboo Search", command=self.ts)
        run_menu.add_command(label="Genetic Algorithm", command=self.ga)
        opt_menu.add_command(label="Load Dataset", command=self.ds)
        # link top menu and sub menu
        menubar.add_cascade(label="Run", menu=run_menu)
        menubar.add_cascade(label="Options", menu=opt_menu)
        # link main window and menu
        self.master["menu"] = menubar

    def ds(self):
        filename = tkFileDialog.askopenfilename()
        if filename:
            Dataset.loaddataset(filename)
            self.cmd.settext(Dataset.name)

    def ga(self):
        self.redirect()
        Solution.ga = True
        ga = GeneticAlgorithm()
        self.setinfo(ga.solve())

    def sa(self):
        self.redirect()
        sa = SimulatedAnnealing()
        self.setinfo(sa.solve())

    def ts(self):
        self.redirect()
        ts = TabooSearch()
        self.setinfo(ts.solve())

    def show_trends(self, data):
        x = data[0]
        y = data[1]
        pyplot.plot(x,y)
        pyplot.xlabel(r'times')
        pyplot.ylabel(r'fitness')
        pyplot.show()

    def redirect(self):
        self.checkdataset()
        mylogger = AppWin.Logger()
        self.stdout_ = sys.stdout
        sys.stdout = mylogger

    def setinfo(self, result):
        global info
        sys.stdout = self.stdout_
        self.cmd.settext(info)
        info = ""
        self.show_trends(result)

    def checkdataset(self):
        if not Dataset.ds:
            Dataset.loaddataset("COMP40511_2010_dataset_4.xls")

    class Logger:
        def write(self, s):
            global info
            info += s
################################################################################

info = ""
def main():
    AppWin().mainloop()

if __name__ == "__main__":
    main()
