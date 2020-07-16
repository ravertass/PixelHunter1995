
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
from matplotlib.collections import PathCollection, LineCollection
from collections import namedtuple
from itertools import chain
from typing import List,Tuple

#endregion

PickEventArgs = namedtuple("pick_event_args", "graph G pos")

class Graph:
  # TODO consider using namedtuples or similar (pairs?), to better allow labels
  nodes : List[List[int]] = [] # List of node-sets (another list)
  edges : List[Tuple[int,int]] = [] # List of edges (a pair)
  
  @property
  def allnodes(self):
    return list(chain.from_iterable(self.nodes))
  
  def __init__(self, nodes : List[List[int]] = None , edges : List[Tuple[int,int]] = None ):
  # def __init__(self, nodes : [[int]] = None , edges : [(int,int)] = None ):
  # def __init__(self, nodes = None, edges=None):
    self.nodes = nodes
    self.edges = edges



class Network():
  
  def __init__(self, graph:Graph = None, network:nx.Graph=None, pos=None):
    # (G, pos, black, white) = generate_graph();
    if graph is None:
      graph = self.test_nodes()
    
    if network is None:
      network = self.generate_network(graph)
      
    if pos is None:
      pos = self.spring_layout(network)
    
    (self.fig, self.ax) = (fig, ax) = plot = plt.subplots()
    ax.axis('off')
    ax.axis((-5,5,-3,3))
    
    # event must be set before refresh is called
    self.pick_event_args = PickEventArgs(graph, network, pos)
    fig.canvas.mpl_connect('pick_event', lambda event: self.onClick(event, *plot, *self.pick_event_args) )
    # fig.canvas.callbacks.connect('button_press_event', self.onClick)
    
    self.refreshGraph(*plot, graph, network, pos)
    
    # plt.show()
  
  def test_nodes(self) -> Graph:
    black, white = [1, 4, 5, 6, 7], [2, 3]
    edges = [(1, 2), (1, 3), (2, 4), (2, 5), (3, 6), (3, 7)]
    return Graph([black, white], edges)
  
  def generate_network(self, graph:Graph) -> nx.Graph:
    G = nx.Graph()
    for node in graph.allnodes:
      G.add_node(node)
    G.add_edges_from(graph.edges)
    # for edge in graph.edges:
    #   G.add_edge(*edge)
    
    return G
  
  def spring_layout(self, graph):
    return nx.spring_layout(graph, k=1, scale=2, seed=0)
  
  def onClick(self, event, fig, ax, graph, G, pos):
    print()
    print("onclick", event, event.artist, event.ind)
    
    black = graph.nodes[0]
    white = graph.nodes[1]
    edges = graph.edges
    # print("vars", vars(event))
    # print("vars", vars(event.artist))
    if isinstance(event.artist, PathCollection):
      node = event.artist.items[event.ind[0]]
      print("node", node)
      if node in black:
        print("black")
        black.remove(node)
        white.append(node)
        self.refreshGraph(fig, ax, graph, G, pos)
      elif node in white:
        print("white")
        white.remove(node)
        black.append(node)
        self.refreshGraph(fig, ax, graph, G, pos)
    elif isinstance(event.artist, LineCollection):
      edge = event.artist.items[event.ind[0]]
      print("edge", edge)
      # print("1", vars(G))
      # print("2", G.adj)
      # print("1", G.edges)
      # print("1.5", type(G.edges))
      # print("3", G._adj[event.ind[0]])
      
      new_node = len(graph.allnodes)+1
      new_edges = [(edge[0],new_node), (edge[1],new_node)]
      print("new_edges", new_edges)
      
      graph.nodes[0].append(new_node)
      graph.edges.remove(edge)
      graph.edges.extend(new_edges)
      print(graph.edges)
      # for edge in new_edges:
      #   graph.edges.append(edge)
      
      G.remove_edge(*edge)
      G.add_node(new_node)
      G.add_edges_from(new_edges)
      # for edge in new_edges:
      #   G.add_edge(*edge)
      
      pos = self.spring_layout(G)
      
      self.refreshGraph(fig, ax, graph, G, pos)
      
      # Would rather return, but the callbacks can't assign the return-value...
      self.pick_event_args = PickEventArgs(graph, G, pos)
      # return self.pick_event_args
  
  def refreshGraph(self, fig, ax, graph:Graph, G, pos):
    # (black_nodes, white_nodes, edges) = refreshGraph(fig, ax, G, pos, black, white)
    bg_color = 'white'
    node_size = 400
    ax.cla()
    ax.patch.set_facecolor(bg_color)
    ax.axis('off')
    
    black = graph.nodes[0]
    white = graph.nodes[1]
    edges = graph.edges
    
    # Draw edges first so we can hide the part drawn inside node
    edge_artist = nx.draw_networkx_edges(G, pos, edgelist=edges, ax=ax, width=1.0, alpha=0.5)
    nx.draw_networkx_nodes(G, pos, alpha=1, ax=ax, node_color=bg_color, node_size=node_size)
    
    black_nodes = nx.draw_networkx_nodes(G, pos, nodelist=black, node_color='k', alpha=0.8, ax=ax, node_size=node_size)
    white_nodes = nx.draw_networkx_nodes(G, pos, nodelist=white, node_color='w', alpha=0.8, node_shape="s", ax=ax, node_size=node_size)
    
    if black_nodes is not None:
      black_nodes.set_edgecolor('k')
    if white_nodes is not None:
      white_nodes.set_edgecolor('k')
    
    # Couples the artist to the data, for the 'pick-event' event
    # the events ind value contains the index
    black_nodes.items = black
    white_nodes.items = white
    edge_artist.items = edges
    # Pickers has to be set before plt.show
    black_nodes.set_picker(True)
    white_nodes.set_picker(True)
    edge_artist.set_picker(True)
    # black_nodes.set_picker(1)
    # white_nodes.set_picker(1)
    # edge_artist.set_picker(1)
    
    # plt.draw()
    # plt.show()
    fig.canvas.draw()
    # fig.canvas.draw_idle()
    # fig.show()
    # ax.show() # doesnt work


class Window_tkinter():
  def __init__(self):
    # G = nx.path_graph(8)
    # E = nx.path_graph(30)

    # # one plot, both graphs
    # fig, ax = plt.subplots()
    # nx.draw(G, ax=ax)
    # nx.draw(E, ax=ax)
    # plt.show()
    
    network = Network()
    
    window=tkinter.Tk("Program Window")
    
    self.text = text = tkinter.Text(window)
    text.grid()
    
    self.canvas = FigureCanvasTkAgg(network.fig, window)
    # get canvas as tkinter's widget and `gird` in widget `window`
    # canvas.get_tk_widget().pack(side=RIGHT, fill = BOTH, expand=1)
    self.canvas.get_tk_widget().grid(row=0, column=1)
    self.canvas.draw()
    
    self.canvas.mpl_connect('pick_event', lambda event: self.onClick(event, network.fig, network.ax, *network.pick_event_args) )
    # self.canvas.mpl_connect('button_press_event', lambda event:
    #     self.onClick(*plot, *graph, event.xdata, event.ydata))
    # window.bind("<Button-1>", onClick)
    
    # pyplot keeps program running unless explicitly closed
    window.protocol("WM_DELETE_WINDOW", lambda: (window.destroy(), plt.close(network.fig)))
    
    window.mainloop()
  
  def onClick(self, event, fig, ax, graph, G, pos):
    print("onclick", event, event.artist, event.ind)
    black = graph.nodes[0]
    white = graph.nodes[1]
    edges = graph.edges
    # print("vars", vars(event))
    # print("vars", vars(event.artist))
    if isinstance(event.artist, PathCollection):
      node = event.artist.items[event.ind[0]]
      self.text.insert('end', 'Node: {}\nedges: {}\n adj: {}\n'.format(node, "dunno", G._adj[node]))
      # self.text.insert(1.0, 'Node: {}\nedges: {}\n adj: {}\n'.format(node, "dunno", G._adj[node]))
      

# Network()
Window_tkinter()