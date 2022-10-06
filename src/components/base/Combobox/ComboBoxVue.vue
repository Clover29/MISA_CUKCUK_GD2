<template>
  <div class="autocomplte" :class="div_class">
    <input
      class="form__text"
      :class="text_class"
      type="text"
      ref="input"
      :value="Name"
      @blur="validate"
    />
    <div
      class="icon icon-16 icon__dropdown"
      :class="dropdown__icon"
      @click="showDropDown"
    ></div>
    <i
      class="fa-solid fa-plus icon__add position__plus"
      :class="plus__icon"
    ></i>
    <div class="combobox__option unit_position" :class="width" v-show="isShow">
      <div
        v-for="item in optionData"
        :key="item.Id"
        class="combobox__option--row"
        :class="{ combobox__highlight: ID == item.Id }"
        @click="onClickOption(item)"
      >
        {{ item.Name }}
      </div>
    </div>
  </div>
</template>
  <style scoped>
@import url(combobox.css);
</style>
   <script>
export default {
  name: "ComboboxVue",
  props: 
  ["div_class",
    "width",
    "plus__icon",
    "dropdown__icon",
    "text_class",
    "UnitList",
    "StockList",
    "ID",
    "Name",
  ],
  data() {
    return {
      isShow: false, // hiển thị option
    }
  },
  watch:{
    Name:function(){
      this.validate();
    }
  },
  computed: {
    optionData() {
      var optionData = [];
      if (this.UnitList) {
        for (let index = 0; index < this.UnitList.length; index++) {
          optionData.push({
            Id: this.UnitList[index].unitID,
            Name: this.UnitList[index].unitName,
          });
        }
      }
      if( this.StockList){
        for (let index = 0; index < this.StockList.length; index++) {
          optionData.push({
            Id: this.StockList[index].stockID,
            Name: this.StockList[index].stockName,
          });
        }
      }
      return optionData;
    },
  },
  methods: {
    showDropDown() {
      this.isShow = !this.isShow;
    },
    onClickOption(value) { 
      this.$emit('update:Name',value.Name );
      this.$emit('update:ID',value.Id );
      this.$emit('updateDescription'); 
      this.isShow = false;
    },
    validate(){
      if(!this.text_class){
        if(this.$refs.input &&!this.Name){
          this.$refs.input.classList.add('border-red');
        }else{
          this.$refs.input.classList.remove('border-red');
        }
      }
    }
  },
};
</script>