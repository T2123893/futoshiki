/**
 * $Id$
 * 
 * SOFT40051 - Object-oriented Programming in C++ Assignment
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 *
 * File:    Triangle.cpp
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */

#pragma once


#include "stdafx.h"
#include "Triangle.h"


void Triangle::perform(GWindow &gw)
{
    //gw.setPenColour();
    gw.triangle(_a.getX(), _a.getY(), _b.getX(), _b.getY(), _c.getX(), _c.getY());    
}

void Triangle::load(GWindow &gw, vector<string> &attr) 
{
    this->_centre = Point(parse(attr[1]), parse(attr[2]));
    _a = Point(parse(attr[3]), parse(attr[4]));
    _b = Point(parse(attr[5]), parse(attr[6]));
    _c = Point(parse(attr[7]), parse(attr[8]));
}

void Triangle::save(fstream &out)
//void Triangle::save(char *fileName)
{
    //fstream out;
    //out.open(fileName, ios_base::app);
    out << "Triangle," << this->_centre.getX() << "," << this->_centre.getY();
    out << "," <<  _a.getX() << "," << _a.getY() << "," << _b.getX() << ","; 
    out << _b.getY() << "," << _c.getX() << "," << _c.getY() << endl;

}

void Triangle::prompt(GWindow &gw)
{
    setCentre(gw);
    
    int x, y;
    gw.writeText(200, 95, "A vertex X: ");
    x = gw.readInt();
    gw.writeText(200, 115, "A vertex Y: ");
    y = gw.readInt();
    setA(Point(x,y));
    
    gw.writeText(200, 135, "B vertex X: ");
    x = gw.readInt();
    gw.writeText(200, 155, "B vertex Y: ");
    y = gw.readInt();
    setB(Point(x,y));

    gw.writeText(200, 175, "The Triangle attributes are set up. Press 'Q' back to main menu");
}

void Triangle::show(GWindow &gw)
{
    gw.writeText("    Triangle                  ");
    printPoint(gw, this->_centre, "Center"); 
    printPoint(gw, _a, " Vertices: A");
    printPoint(gw, _b, "  B");
    printPoint(gw, _c, "  C");
}

Point Triangle::getA()
{
    return _a;
}

void Triangle::setA(Point a)
{
    _a = a;
    setC(a, _b);
   
}

Point Triangle::getB()
{
    return _b;
}

void Triangle::setB(Point b)
{
    _b = b;
    setC(_a, _b);
}

Point Triangle::getC()
{
    return _c;
}


void Triangle::setC(Point a, Point b)
{    
    _c.setX(abs(_centre.getX()*3 - a.getX() - b.getX()));    
    _c.setY(abs(_centre.getY()*3 - a.getY() - b.getY()));
}

Triangle::Triangle(Point a, Point b)
{
    _a = a;
    _b = b;    
    setC(a, b);
}

Triangle::Triangle(void)
{
}

Triangle::~Triangle(void)
{
    cout << "三角形被删了";
}


//int main()
//{
//    Triangle t;
//    //Triangle t(Point(1,5), Point(4,3));
//
//    t.setA(Point(-3,3));
//    t.setB(Point(0,3));
//
//    cout << "Centre(" << t.getCentre().getX() << "," << t.getCentre().getY() << ")" << endl;
//    cout << "A(" << t.getA().getX() << "," << t.getA().getY() << ")" << endl;
//    cout << "B(" << t.getB().getX() << "," << t.getB().getY() << ")" << endl;
//    cout << "C(" << t.getC().getX() << "," << t.getC().getY() << ")" << endl;
//
//
//    
//    return 0;
//}