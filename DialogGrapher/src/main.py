
#region imports

import sys
import os
import platform
import argparse
import networkx as nx
import matplotlib.pyplot as plt
import numpy
import tkinter
import pygame

from tkinter import messagebox
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
from collections import namedtuple

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
  
  # test_tkinter_pygame()
  # test_tkinter_networkx()
  test_pyplot_networkx()
  # test_pygame()


def generate_graph():
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
  pos=nx.spring_layout(G, k=1, scale=2)
  
  return (G, pos, black, white)

def refreshGraph(fig, ax, G, pos, black, white):
  # plt.clf()
  # plt.cla()
  ax.cla()
  ax.patch.set_facecolor('white')
  ax.axis('off')
  ax.axis((-5,5,-3,3))
  
  black_nodes = nx.draw_networkx_nodes(G, pos, ax=ax, nodelist=black, node_color='k', node_size=400, alpha=0.8)
  white_nodes = nx.draw_networkx_nodes(G, pos, ax=ax, nodelist=white, node_color='w', node_size=400, alpha=0.8, node_shape="s")
  edges = nx.draw_networkx_edges(G, pos, ax=ax, width=1.0, alpha=0.5)
  if white_nodes is not None:
    white_nodes.set_edgecolor('k')
  # plt.axis('off')
  # plt.axis((-4,4,-1,3))
  
  black_nodes.items = black
  white_nodes.items = white
  return (black_nodes, white_nodes, edges)

def onClick_networkx(fig, ax, G, pos, black, white, x, y):
  print("onClick_networkx ", x, y)
  refresh = False
  
  allNodes = white+black
  for i in allNodes:
    node = pos[i]
    distance = pow(x-node[0],2)+pow(y-node[1],2)
    if distance < pow(0.25,2):
      if i in black:
        print("black")
        black.remove(i)
        white.append(i)
      elif i in white:
        print("white")
        white.remove(i)
        black.append(i)
      refresh = True
  return refresh


class test_tkinter_pygame():
  Done = False
  
  def __init__(self):
    ScreenSize = namedtuple("ScreenSize", "width height")
    screen_size = ScreenSize(500,500)
    
    graph = generate_graph()
    # refreshGraph(*plot, *graph)
    
    #initialise tkinter
    self.root = root = tkinter.Tk("Program Window")
    root.protocol("WM_DELETE_WINDOW", self.quit_callback)
    
    self.embed = embed = tkinter.Frame(root, width=screen_size.width, height=screen_size.height)
    embed.grid(columnspan = screen_size.width, rowspan = screen_size.height)
    embed.pack(side = tkinter.LEFT)
    
    right_frame = tkinter.Frame(root, width = screen_size.width, height = screen_size.height)
    # right_frame.grid(columnspan = screen_size.width, rowspan = screen_size.height)
    right_frame.pack(side = tkinter.RIGHT)
    
    text = tkinter.Text(right_frame)
    
    # root.mainloop()
    
    #This embeds the pygame window
    os.environ['SDL_WINDOWID'] = str(self.embed.winfo_id())
    if platform.system == "Windows":
      os.environ['SDL_VIDEODRIVER'] = 'windib'
    
    # initialise pygame
    pygame.init()
    surface = pygame.display.set_mode(screen_size)
    # surface.fill((50, 50, 50))
    
    self.color = (255,100,100)
    self.rect = pygame.draw.rect(surface, pygame.Color(*self.color), (100, 100, 300, 50))
    pygame.display.update()
    
    # start pygame clock
    clock = pygame.time.Clock()
    gameframe = 0
    while not self.Done:
      try:
        embed.update()
      except:
        print("dialog error")
      
      for event in pygame.event.get():
        if event.type == pygame.MOUSEBUTTONDOWN and event.button == 1:
          pos = pygame.mouse.get_pos()
          if self.rect.collidepoint(pos):
            print("click!")
            self.color = self.color[1:] + self.color[0:1]
            self.rect = pygame.draw.rect(surface, pygame.Color(*self.color), (self.rect.left, self.rect.top, self.rect.right-self.rect.left, self.rect.bottom-self.rect.top))
            pygame.display.update()
        if event.type == pygame.QUIT:
          print("pygame quit!")
          self.quit_callback()
      
      if self.Done:
        break
      
      self.Draw(surface)
      
      clock.tick(30)
      gameframe += 1
    
    
    # pyplot keeps program running unless explicitly closed
  
  def Draw(self, surface):
    self.root.update()
    #Clear view
    surface.fill((80,80,80))
    
    self.rect = pygame.draw.rect(surface, pygame.Color(*self.color), (self.rect.left, self.rect.top, self.rect.right-self.rect.left, self.rect.bottom-self.rect.top))
    
    pygame.display.flip()
  
  def onClick(self, fig, ax, G, pos, black, white, x, y):
    print("onclick", x, y)
    
  def quit_callback(self):
    print("quit_callback!")
    self.Done = True
    self.root.destroy()


class test_tkinter_networkx():
  def __init__(self):
    # G = nx.path_graph(8)
    # E = nx.path_graph(30)

    # # one plot, both graphs
    # fig, ax = plt.subplots()
    # nx.draw(G, ax=ax)
    # nx.draw(E, ax=ax)
    # plt.show()
    
    plot = plt.subplots()
    fig = plot[0]
    
    graph = generate_graph()
    refreshGraph(*plot, *graph)
    
    window=tkinter.Tk("Program Window")
    
    text=tkinter.Text(window)
    text.grid()
    
    self.canvas = FigureCanvasTkAgg(fig, window)
    # get canvas as tkinter's widget and `gird` in widget `window`
    # canvas.get_tk_widget().pack(side=RIGHT, fill = BOTH, expand=1)
    self.canvas.get_tk_widget().grid(row=0, column=1)
    self.canvas.draw()
    
    self.canvas.mpl_connect('button_press_event', lambda event:
        self.onClick(*plot, *graph, event.xdata, event.ydata))
    # window.bind("<Button-1>", onClick)
    
    # pyplot keeps program running unless explicitly closed
    window.protocol("WM_DELETE_WINDOW", lambda: (window.destroy(), plt.close(fig)))
    
    window.mainloop()
  
  def onClick(self, fig, ax, G, pos, black, white, x, y):
    print("onclick", x, y)
    if (onClick_networkx(fig, ax, G, pos, black, white, x, y) == True):
      refreshGraph(fig, ax, G, pos, black, white)
      self.canvas.draw()


class test_pyplot_networkx():
  def __init__(self):
    # (G, pos, black, white) = generate_graph();
    graph = generate_graph()
    # fig, ax = plt.subplots()
    plot = plt.subplots()
    fig = plot[0]
    # fig.canvas.mpl_connect('button_press_event', lambda event:
    #     (print(),
    #     print('button_press_event', event),
    #     self.onClick(*plot, *graph, event.xdata, event.ydata)))
    # event must be set before refresh
    fig.canvas.mpl_connect('pick_event', lambda event:
        (print(),
        # print('pick_event', event, event.artist, event.ind, event.mouseevent),
        # self.onClick(*plot, *graph, event.mouseevent.xdata, event.mouseevent.ydata)))
        self.onClick(*plot, *graph, event)))
    # fig.canvas.callbacks.connect('button_press_event', self.onClick)
    
    self.refreshGraph(*plot, *graph)
  
  def onClick(self, fig, ax, G, pos, black, white, event):
    print("onclick", event, event.artist, event.ind)
    # print("vars", vars(event))
    # print("vars", vars(event.artist))
    if hasattr(event.artist, 'items'):
      node = event.artist.items[event.ind[0]]
      print("node", node)
      if node in black:
        print("black")
        black.remove(node)
        white.append(node)
        self.refreshGraph(fig, ax, G, pos, black, white)
      elif node in white:
        print("white")
        white.remove(node)
        black.append(node)
        self.refreshGraph(fig, ax, G, pos, black, white)
    else:
      edge = G._adj[event.ind[0]] # _adj is wrong, isnt the list of edges
      print("edge", edge)
    
    # allNodes = white+black
    # for i in allNodes:
    #   node = pos[i]
    #   distance = pow(x-node[0],2)+pow(y-node[1],2)
    #   if distance < pow(0.25,2):
    #     if i in black:
    #       print("black")
    #       black.remove(i)
    #       white.append(i)
    #     elif i in white:
    #       print("white")
    #       white.remove(i)
    #       black.append(i)
    # return refresh
    
    # if (onClick_networkx(fig, ax, G, pos, black, white, x, y) == True):
    #   self.refreshGraph(fig, ax, G, pos, black, white)
  
  def refreshGraph(self, fig, ax, G, pos, black, white):
    (black_nodes, white_nodes, edges) = refreshGraph(fig, ax, G, pos, black, white)
    # Pickers has to be set before plt.show
    # black_nodes.set_picker(True)
    # white_nodes.set_picker(True)
    # edges.set_picker(True)
    black_nodes.set_picker(1)
    white_nodes.set_picker(1)
    edges.set_picker(1)
    plt.show()
    # for node in black_nodes:
    #   node.set_picker(True)


class test_pygame():
  
  def __init__(self):
    pygame.init()
    width = 500
    height = 500
    window = pygame.display.set_mode((width, height))
    window.fill((50, 50, 50))
    
    self.color = (255,100,100)
    self.rect = pygame.draw.rect(window, pygame.Color(*self.color), (100, 100, 200, 25))
    pygame.display.update()
    
    running = True
    while running:
      for event in pygame.event.get():
        if event.type == pygame.MOUSEBUTTONDOWN and event.button == 1:
          pos = pygame.mouse.get_pos()
          if self.rect.collidepoint(pos):
            print("click!")
            self.color = self.color[1:] + self.color[0:1]
            self.rect = pygame.draw.rect(window, pygame.Color(*self.color), (100, 100, 200, 25))
            pygame.display.update()
        if event.type == pygame.QUIT:
          running = False


#region main()-boilerplate
if __name__ == "__main__":
  main(sys.argv)
#endregion
