#include <GL/gl.h>
#include <GL/glut.h>
#include "erica.hpp"
#include "BasicTPS.hpp"
#include <iostream>
using namespace erica;
using namespace std;

const int ACTOR_ID_MY = 1;

GameLogic*  logic = NULL;
GameView*   view  = NULL;
Actor*      actr  = NULL;
Controller* ctrl  = NULL;


static 
void init_game ()
{
    logic = new BasicTPSLogic;
    view  = new BasicTextPlayerView (logic);
    actr  = new BasicActor;
    ctrl  = new BasicController;

    logic->add_actor (actr);
    view->add_controller (ctrl);
    actr->set_actor_id (ACTOR_ID_MY);
    ctrl->set_actor_id (ACTOR_ID_MY);

    EventManager:: regist (100, "KEY_PRESSED");
    EventManager:: regist (101, "WALK");
    EventManager:: regist (102, "SCENE_NODE_MOVE");
    EventManager:: regist (103, "GAME_QUIT");
}

static
void keyboard (unsigned char key, int x, int y)
{ 
    KeyPressedParams params;

    switch (key) {
    case '4': {
        params.key = '4';
        view->enqueue (new Event("KEY_PRESSED", &params, sizeof(params), 0));
    }
    case 'q': {
        params.key = 'q';
        view->enqueue (new Event("KEY_PRESSED", &params, sizeof(params), 0));
    }
    }
}

static
void display ()
{
}

static
void idle ()
{
    static int t = 0;
    logic->update (t++);
    
    if (logic->end_of_game()) {
        cout << "end of game.\n";
        exit (0);
    }
}

int main (int argc, char** argv)
{
    glutInit            (&argc, argv);
    glutInitDisplayMode (GLUT_RGB | GLUT_DOUBLE | GLUT_DEPTH);
    glutInitWindowSize  (640, 480);
    glutCreateWindow    ("MyOpenGL");
    glutKeyboardFunc (keyboard);
    glutDisplayFunc  (display);
    glutIdleFunc     (idle);

    init_game ();

    glutMainLoop();
    return 0;
}
