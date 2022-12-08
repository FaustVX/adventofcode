original source: [https://adventofcode.com/2022/day/8](https://adventofcode.com/2022/day/8)
## --- Day 8: Treetop Tree House ---
The expedition comes across a peculiar patch of tall trees all planted carefully in a grid. The Elves explain that a previous expedition planted these trees as a reforestation effort. Now, they're curious if this would be a good location for a [tree house](https://en.wikipedia.org/wiki/Tree_house).

First, determine whether there is enough tree cover here to keep a tree house <em>hidden</em>. To do this, you need to count the number of trees that are <em>visible from outside the grid</em> when looking directly along a row or column.

The Elves have already launched a [quadcopter](https://en.wikipedia.org/wiki/Quadcopter) to generate a map with the height of each tree (your puzzle input). For example:

<pre>
<code>30373
25512
65332
33549
35390
</code>
</pre>

Each tree is represented as a single digit whose value is its height, where <code>0</code> is the shortest and <code>9</code> is the tallest.

A tree is <em>visible</em> if all of the other trees between it and an edge of the grid are <em>shorter</em> than it. Only consider trees in the same row or column; that is, only look up, down, left, or right from any given tree.

All of the trees around the edge of the grid are <em>visible</em> - since they are already on the edge, there are no trees to block the view. In this example, that only leaves the <em>interior nine trees</em> to consider:


 - The top-left <code>5</code> is <em>visible</em> from the left and top. (It isn't visible from the right or bottom since other trees of height <code>5</code> are in the way.)
 - The top-middle <code>5</code> is <em>visible</em> from the top and right.
 - The top-right <code>1</code> is not visible from any direction; for it to be visible, there would need to only be trees of height <em>0</em> between it and an edge.
 - The left-middle <code>5</code> is <em>visible</em>, but only from the right.
 - The center <code>3</code> is not visible from any direction; for it to be visible, there would need to be only trees of at most height <code>2</code> between it and an edge.
 - The right-middle <code>3</code> is <em>visible</em> from the right.
 - In the bottom row, the middle <code>5</code> is <em>visible</em>, but the <code>3</code> and <code>4</code> are not.

With 16 trees visible on the edge and another 5 visible in the interior, a total of <code><em>21</em></code> trees are visible in this arrangement.

Consider your map; <em>how many trees are visible from outside the grid?</em>


