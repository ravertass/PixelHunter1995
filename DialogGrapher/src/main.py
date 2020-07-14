
#region imports

import sys
import argparse
import networkx as nx
import matplotlib.pyplot as plt
import numpy
import pygame


#endregion


def parse_args():
  parser = argparse.ArgumentParser()
  #
  # ... configure command line arguments ...
  #
  parser.add_argument('-d', '--dir', nargs=1, help='The directory of the dialog to edit.', required=True)
  return parser.parse_args()

def main(argv):
  for arg in argv:
    print('argv: ' + arg)
  args = parse_args()
  
  test_networkx()

def test_networkx():
  
  G = nx.Graph()
  black, white = [1, 4, 5, 6, 7], [2, 3]
  allNodes = white+black
  for node in allNodes:
    G.add_node(node)
  G.add_edge(1, 2)
  G.add_edge(1, 3)
  G.add_edge(2, 4)
  G.add_edge(2, 5)
  G.add_edge(3, 6)
  G.add_edge(3, 7)
  pos=nx.spring_layout(G)
  
  fig, ax = plt.subplots()
  def onClick(event):
    print("onclick closure")
    onClick_networkx(G, pos, black, white, fig, event)
  fig.canvas.mpl_connect('button_press_event', onClick)
  
  refreshGraph(G, pos, black, white, fig)

def refreshGraph(G, pos, black, white, fig):
  plt.clf()
  nx.draw_networkx_nodes(G, pos, nodelist=black, node_color='k', node_size=400, alpha=0.8)
  white_nodes = nx.draw_networkx_nodes(G, pos, nodelist=white, node_color='w', node_size=400, alpha=0.8)
  nx.draw_networkx_edges(G,pos,width=1.0, alpha=0.5)
  if white_nodes is not None:
    white_nodes.set_edgecolor('k')
  plt.axis('off')
  plt.axis((-4,4,-1,3))
  fig.patch.set_facecolor('white')
  plt.show()

def onClick_networkx(G, pos, black, white, fig, event):
  print("onClick_networkx")
  (x,y)   = (event.xdata, event.ydata)

  allNodes = white+black
  for i in allNodes:
    node = pos[i]
    distance = pow(x-node[0],2)+pow(y-node[1],2)
    if distance < 0.1:
      print("onclick!!!")
      if i in black:
        print("black")
        black.remove(i)
        white.append(i)
      elif i in white:
        print("white")
        white.remove(i)
        black.append(i)
      refreshGraph(G, pos, black, white, fig)

def test_pygame():
  pygame.init()
  width = 500
  height = 500
  window = pygame.display.set_mode((width, height))
  window.fill((50, 50, 50))
  rect = pygame.draw.rect(window, pygame.Color(255,100,100), (100, 100, 200, 25))
  pygame.display.update()
  running = True
  while running:
    for event in pygame.event.get():
      if event.type == pygame.MOUSEBUTTONDOWN and event.button == 1:
        pos = pygame.mouse.get_pos()
        if rect.collidepoint(pos):
          print("click!")
      if event.type == pygame.QUIT:
        running = False

#region main-boilerplate
if __name__ == "__main__":
  main(sys.argv)
#endregion
