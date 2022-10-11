<template>
  <div class="paging">
    <div class="paging__left">
      <i
        class="fa-solid fa-angles-left paging__text"
        @click="
          pageNumber = 1;
          isEnter = false;
        "
      ></i>
      <i
        class="fa-solid fa-chevron-left paging__text"
        @click="
          if (pageNumber > 1) pageNumber--;
          isEnter = false;
        "
      ></i>
      <div class="paging__text icon__line">|</div>
      Trang
      <input
        type="text"
        name="paging"
        v-model="pageNumber"
        class="paging_number"
        @keydown.enter="loadData"
      />
      trên
      {{ totalPage }}
      <div class="paging__text icon__line" style="margin-left: 8px">|</div>
      <i
        class="fa-solid fa-chevron-right paging__text"
        @click="
          if (pageNumber < totalPage) pageNumber++;
          isEnter = false;
        "
      ></i>
      <i
        class="fa-solid fa-angles-right paging__text"
        @click="
          pageNumber = totalPage;
          isEnter = false;
        "
      ></i>
      <div class="paging__text icon__line">|</div>
      <i class="fa-solid fa-rotate-right paging__text" @click="loadData"></i>
      <div class="paging__text icon__line">|</div>
      <SelectionBox
        :isSelectShow="true"
        :value="pageSize"
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
      Hiển thị {{ firstRecord }} - {{ lastRecord }} trên {{ totalRecord }} kết
      quả
    </div>
  </div>
</template>
<style>
@import url(paging.css);
</style>
<script>
import { Resources } from "@/js/Resources";
import { MISAEnum } from "../../../js/Enum.js";
import SelectionBox from "../SelectionBox/SelectionBox.vue";
export default {
  name: "MaterialPaging",
  components: { SelectionBox },
  emits: ["LoadData"],
  props: [
    "totalRecord",
    "totalPage",
    "materialFillter",
    "pageSize",
    "PageNumber",
  ],
  data() {
    return {
      option: [
        { id: 1, value: 10, text: 10 },
        { id: 2, value: 25, text: 25 },
        { id: 3, value: 50, text: 50 },
        { id: 4, value: 100, text: 100 },
      ], //list số bản ghi muốn hiển thị
      type: MISAEnum.OptionType.Paging, //loại option hiển thị
      pageNumber: this.PageNumber, // trang hiện tại
      firstRecord: 1, // bản ghi đầu tiên của 1 trang
      lastRecord: this.pageSize, // bản ghi cuối của 1 trang
      isEnter: true, // kiểm tra phím enter được ấn hay không
    };
  },
  watch: {
    pageNumber: function () {
      if (this.pageNumber && this.pageNumber > 0 && !this.isEnter) {
        this.loadData();
      }
      this.firstRecord =
        this.firstRecord > 0 ? (this.pageNumber - 1) * this.pageSize + 1 : 1;
      this.lastRecord =
        this.firstRecord + this.pageSize - 1 > this.totalRecord
          ? this.totalRecord
          : this.firstRecord + this.pageSize - 1;
    },
  },
  methods: {
    getPageSize(pageSize) {
      try {
        this.isEnter = false;
        this.$emit(Resources.EMIT_UPDATE_PAGESIZE, pageSize);
        this.pageNumber = 1;
        this.lastRecord = pageSize;
        this.$emit(  Resources.EMIT_LOAD_DATA, pageSize, this.pageNumber, this.materialFillter);
      } catch (error) {
        console.log(error);
      }
    },
    loadData() {
      try {
        this.isEnter = true;
        this.$emit(
          Resources.EMIT_LOAD_DATA,
          this.pageSize,
          this.pageNumber,
          this.materialFillter
        );
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>