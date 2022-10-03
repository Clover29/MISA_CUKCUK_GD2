<template>
  <div class="paging">
    <div class="paging__left">
      <i class="fa-solid fa-angles-left paging__text"></i>
      <i class="fa-solid fa-chevron-left paging__text"></i>
      <div class="paging__text icon__line">|</div>
      Trang
      <input
        type="text"
        name="paging"
        v-model="pageNumber"
        class="paging_number"
        @input="loadData"
      />
      trên
      {{ totalPage }}
      <div class="paging__text icon__line" style="margin-left: 8px">|</div>
      <i class="fa-solid fa-chevron-right paging__text"></i>
      <i class="fa-solid fa-angles-right paging__text"></i>
      <div class="paging__text icon__line">|</div>
      <i class="fa-solid fa-rotate-right paging__text"></i>
      <div class="paging__text icon__line">|</div>
      <SelectionBox
        :isSelectShow="true"
        :value="'10'"
        :input_class="'paging_number'"
        :icon_class="'dropdown_page_position'"
        :option_class="'paging__position'"
        :option="option"
        :optionType="type"
        :optionID="option[0].id"
        @PageSize="getPageSize"
      />
    </div>
    <div class="paging__right">
      Hiển thị 1-{{ pageSize }} trên {{ totalRecord }} kết quả
    </div>
  </div>
</template>
<style>
@import url(paging.css);
</style>
<script>
import { MISAEnum } from "../../../js/Enum.js";
import SelectionBox from "../SelectionBox/SelectionBox.vue";
export default {
  name: "MaterialPaging",
  components: { SelectionBox },
  emits:['LoadData'],
  props: ["totalRecord", "totalPage", "materialFillter"],
  data() {
    return {
      option: [
        { id: 1, value: 10, text:10 },
        { id: 2, value: 25, text:25 },
        { id: 3, value: 50, text:50 },
        { id: 4, value: 100, text:100 },
      ],
      type: MISAEnum.OptionType.Paging,
      pageSize: 10,
      pageNumber: 1,
      debounce: 0,
    };
  },
  methods: {
    getPageSize(pageSize) {
      this.pageSize = pageSize;
      this.$emit("LoadData", pageSize, this.pageNumber, this.materialFillter);
    },
    loadData() {
      try {
        clearTimeout(this.debounce);
        this.debounce = setTimeout(() => {
          this.$emit(
            "LoadData",
            this.pageSize,
            this.pageNumber,
            this.materialFillter
          );
        }, 600);
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>