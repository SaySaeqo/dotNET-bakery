export default { 
    parse(date) {
        return date.replace(/(....)-(..)-(..)T(..):(..):(..)(.*)Z/, '$1-$2-$3 $4:$5:$6');
    }
}