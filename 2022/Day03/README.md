original source: [https://adventofcode.com/2022/day/3](https://adventofcode.com/2022/day/3)
## --- Day 3: Rucksack Reorganization ---
One Elf has the important job of loading all of the [rucksacks](https://en.wikipedia.org/wiki/Rucksack) with supplies for the jungle journey. Unfortunately, that Elf didn't quite follow the packing instructions, and so a few items now need to be rearranged.

Each rucksack has two large <em>compartments</em>. All items of a given type are meant to go into exactly one of the two compartments. The Elf that did the packing failed to follow this rule for exactly one item type per rucksack.

The Elves have made a list of all of the items currently in each rucksack (your puzzle input), but they need your help finding the errors. Every item type is identified by a single lowercase or uppercase letter (that is, <code>a</code> and <code>A</code> refer to different types of items).

The list of items for each rucksack is given as characters all on a single line. A given rucksack always has the same number of items in each of its two compartments, so the first half of the characters represent items in the first compartment, while the second half of the characters represent items in the second compartment.

For example, suppose you have the following list of contents from six rucksacks:

<pre>
<code>vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw
</code>
</pre>


 - The first rucksack contains the items <code>vJrwpWtwJgWrhcsFMMfFFhFp</code>, which means its first compartment contains the items <code>vJrwpWtwJgWr</code>, while the second compartment contains the items <code>hcsFMMfFFhFp</code>. The only item type that appears in both compartments is lowercase <code><em>p</em></code>.
 - The second rucksack's compartments contain <code>jqHRNqRjqzjGDLGL</code> and <code>rsFMfFZSrLrFZsSL</code>. The only item type that appears in both compartments is uppercase <code><em>L</em></code>.
 - The third rucksack's compartments contain <code>PmmdzqPrV</code> and <code>vPwwTWBwg</code>; the only common item type is uppercase <code><em>P</em></code>.
 - The fourth rucksack's compartments only share item type <code><em>v</em></code>.
 - The fifth rucksack's compartments only share item type <code><em>t</em></code>.
 - The sixth rucksack's compartments only share item type <code><em>s</em></code>.

To help prioritize item rearrangement, every item type can be converted to a <em>priority</em>:


 - Lowercase item types <code>a</code> through <code>z</code> have priorities 1 through 26.
 - Uppercase item types <code>A</code> through <code>Z</code> have priorities 27 through 52.

In the above example, the priority of the item type that appears in both compartments of each rucksack is 16 (<code>p</code>), 38 (<code>L</code>), 42 (<code>P</code>), 22 (<code>v</code>), 20 (<code>t</code>), and 19 (<code>s</code>); the sum of these is <code><em>157</em></code>.

Find the item type that appears in both compartments of each rucksack. <em>What is the sum of the priorities of those item types?</em>


